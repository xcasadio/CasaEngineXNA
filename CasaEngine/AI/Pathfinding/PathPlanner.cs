using CasaEngine.AI.Graphs;
using CasaEngine.AI.Messaging;
using CasaEngine.AI.Navigation;
using Microsoft.Xna.Framework;


namespace CasaEngine.AI.Pathfinding
{
    public class PathPlanner<T>
        where T : WeightedEdge
    {

        public const int NoNodeFound = -1;



        protected internal MovingEntity Owner;

        protected internal Graph<NavigationNode, T> Graph;

        protected internal float NeighboursSearchRange;

        protected internal GraphSearchAlgorithm<NavigationNode, T> Search;

        protected internal Vector3 Destination;

        protected internal PathSmoother SmoothAlgorithm;



        public PathPlanner(MovingEntity owner, Graph<NavigationNode, T> graph, GraphSearchAlgorithm<NavigationNode, T> search, float neighboursSearchRange, PathSmoother smoothAlgorithm)
        {
            this.owner = owner;
            this.graph = graph;
            this.search = search;
            this.neighboursSearchRange = neighboursSearchRange;
            this.SmoothAlgorithm = smoothAlgorithm;
        }



        public MovingEntity Owner => owner;

        public Graph<NavigationNode, T> Graph
        {
            get => graph;
            set => graph = value;
        }

        public GraphSearchAlgorithm<NavigationNode, T> Search
        {
            get => search;
            set => search = value;
        }

        public float NeighboursSearchRange
        {
            get => neighboursSearchRange;
            set => neighboursSearchRange = value;
        }



        public bool RequestPathToPosition(Vector3 position)
        {
            int closestNodeToEntity, closestNodeToDestination;

            PathManager<T>.Instance.Unregister(this);

            Destination = position;

            //If the entity can reach the destination directly, there�s no need to request a search
            if (owner.CanMoveBetween(owner.Position, Destination) == true)
            {
                MessageManagerRouter.Instance.SendMessage(0, owner.ID, 0, (int)MessageType.PathReady, null);
                return true;
            }

            //Get the closest node to the entity
            closestNodeToEntity = ClosestNodeToPosition(owner.Position);
            if (closestNodeToEntity == NoNodeFound)
                return false;

            //Get the cloest node to the destination
            closestNodeToDestination = ClosestNodeToPosition(Destination);
            if (closestNodeToDestination == NoNodeFound)
                return false;

            //Initialize the search
            search.Initialize(closestNodeToEntity, closestNodeToDestination);

            //Register the search in the PathManager
            PathManager<T>.Instance.Register(this);

            return true;
        }

        public List<Vector3> PathOfPositions
        {
            get
            {
                List<Vector3> path;

                path = NodesToPositions(search.PathOfNodes);
                path.Add(Destination);

                return path;
            }
        }

        public List<NavigationEdge> PathOfEdges
        {
            get
            {
                List<int> pathNodes;
                List<NavigationEdge> pathEdges;

                //Get the path of nodes and transform it to a path of edges
                pathNodes = search.PathOfNodes;
                pathEdges = NodesToPathEdges(pathNodes);

                //If there�s at least one node in the path
                if (pathNodes.Count != 0)
                {
                    //Add the edge between the position of the owner entity and the first node of the path
                    pathEdges.Insert(0, new NavigationEdge(owner.Position, graph.GetNode(pathNodes[0]).Position, EdgeInformation.Normal));

                    //Add the edge between the last node of the path and the destination
                    pathEdges.Add(new NavigationEdge(graph.GetNode(pathNodes[pathNodes.Count - 1]).Position, Destination, EdgeInformation.Normal));

                    //Smooth the path
                    SmoothAlgorithm(owner, pathEdges);
                }

                else //The path is a straight line
                    pathEdges.Add(new NavigationEdge(owner.Position, Destination, EdgeInformation.Normal));

                return pathEdges;
            }
        }

        public List<Vector3> GetNowPathOfPositionsToPosition(Vector3 position)
        {
            int closestNodeToEntity, closestNodeToDestination;

            Destination = position;

            //If the entity can reach the destination directly, there�s no need to request a search
            if (owner.CanMoveBetween(owner.Position, Destination) == true)
                return PathOfPositions;

            //Get the closest node to the entity
            closestNodeToEntity = ClosestNodeToPosition(owner.Position);
            if (closestNodeToEntity == NoNodeFound)
                return null;

            //Get the cloest node to the destination
            closestNodeToDestination = ClosestNodeToPosition(Destination);
            if (closestNodeToDestination == NoNodeFound)
                return null;

            //Do the full search and if succesful return the path of positions
            search.Initialize(closestNodeToEntity, closestNodeToEntity);
            if (search.Search() == SearchState.CompletedAndFound)
                return PathOfPositions;

            return null;
        }

        public List<NavigationEdge> GetNowPathOfEdgesToPosition(Vector3 position)
        {
            int closestNodeToEntity, closestNodeToDestination;

            Destination = position;

            //If the entity can reach the destination directly, there�s no need to request a search
            if (owner.CanMoveBetween(owner.Position, Destination) == true)
                return PathOfEdges;

            //Get the closest node to the entity
            closestNodeToEntity = ClosestNodeToPosition(owner.Position);
            if (closestNodeToEntity == NoNodeFound)
                return null;

            //Get the cloest node to the destination
            closestNodeToDestination = ClosestNodeToPosition(Destination);
            if (closestNodeToDestination == NoNodeFound)
                return null;

            //Do the full search and if succesful return the path of positions
            search.Initialize(closestNodeToEntity, closestNodeToEntity);
            if (search.Search() == SearchState.CompletedAndFound)
                return PathOfEdges;

            return null;
        }

        protected internal SearchState CycleOnce()
        {
            SearchState result;

            result = search.CycleOnce();

            //If the search failed inform the owner
            if (result == SearchState.CompletedAndNotFound)
                MessageManagerRouter.Instance.SendMessage(0, owner.ID, 0, (int)MessageType.PathNotAvailable, null);

            //If the search succeeded inform the owner
            if (result == SearchState.CompletedAndFound)
                MessageManagerRouter.Instance.SendMessage(0, owner.ID, 0, (int)MessageType.PathReady, null);

            return result;
        }



        private int ClosestNodeToPosition(Vector3 position)
        {
            int closestNode;
            float distance, closestDistance;
            List<NavigationNode> neighbours;

            //Ask the graph for the neighbour nodes to the position given a search distance
            neighbours = graph.GetNeighbourNodes(0, position, neighboursSearchRange);

            closestNode = NoNodeFound;
            closestDistance = float.MaxValue;

            //See the closest node the entity can reach
            for (int i = 0; i < neighbours.Count; i++)
            {
                //If the entity can reach the position update the closest node values
                if (owner.CanMoveBetween(position, neighbours[i].Position) == true)
                {
                    distance = (position - neighbours[i].Position).LengthSquared();

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNode = i;
                    }
                }
            }

            return closestNode;
        }

        private List<Vector3> NodesToPositions(List<int> nodes)
        {
            List<Vector3> path;

            path = new List<Vector3>();

            //Get the position of each node
            for (int i = 0; i < nodes.Count; i++)
                path.Add(graph.GetNode(i).Position);

            return path;
        }

        private List<NavigationEdge> NodesToPathEdges(List<int> nodes)
        {
            List<NavigationEdge> path;
            Graph<NavigationNode, AnnotatedEdge> annotatedGraph;

            path = new List<NavigationEdge>();

            //Test if the graph is an annotated graph or not
            annotatedGraph = graph as Graph<NavigationNode, AnnotatedEdge>;
            if (annotatedGraph == null)
            {
                //Create the list without extra edge information
                for (int i = 0; i < nodes.Count - 1; i++)
                    path.Add(new NavigationEdge(graph.GetNode(nodes[i]).Position, graph.GetNode(nodes[i + 1]).Position, EdgeInformation.Normal));
            }

            else
            {
                //Create the list with extra edge information
                for (int i = 0; i < nodes.Count - 1; i++)
                    path.Add(new NavigationEdge(annotatedGraph.GetNode(nodes[i]).Position, annotatedGraph.GetNode(nodes[i + 1]).Position, annotatedGraph.GetEdge(nodes[i], nodes[i + 1]).Information));
            }

            return path;
        }

    }
}
