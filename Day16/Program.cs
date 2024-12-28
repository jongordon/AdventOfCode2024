// Input
string[] input = File.ReadAllText("input.txt").Replace("\r\n", "\n").Split('\n');

var dirs = new int[4, 2] { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };
var nodes = new List<Node>();
var start = new Node(0, 0, true, false);
var end = new Node(0, 0, false, true);

// Part 1
var minCost = initSearch(-1, -1);
Console.WriteLine(minCost);

// Part 2
var shortestPath = new List<Node> { end };
buildPath(shortestPath, end);

var seats = new HashSet<Tuple<int, int>>();
foreach (var node in shortestPath)
{
    seats.Add(new Tuple<int, int>(node.x, node.y));
    if (!node.start && !node.end && node.connections.Count != 2)
    {
        var cost = initSearch(node.x, node.y);
        if (cost == minCost)
        {
            var path = new List<Node>();
            buildPath(path, end);

            foreach (var newNode in path)
            {
                seats.Add(new Tuple<int, int>(newNode.x, newNode.y));
            }
        }
    }
}
Console.WriteLine(seats.Count + 1); // +1 for the starting point

int initSearch(int blockC, int blockR)
{
    nodes = [];

    for (var r = 0; r < input.Length; r++)
    {
        for (var c = 0; c < input[0].Length; c++)
        {
            if (!input[r][c].Equals('#') && !(r == blockR && c == blockC))
            {
                var node = new Node(c, r, input[r][c].Equals('S'), input[r][c].Equals('E'));
                nodes.Add(node);

                if (node.start)
                {
                    start = node;
                }
                else if (node.end)
                {
                    end = node;
                }
            }
        }
    }

    // Create connections between adjacent nodes
    foreach (var node in nodes)
    {
        foreach (var otherNode in nodes)
        {
            if (node != otherNode &&
                ((Math.Abs(otherNode.x - node.x) <= 1 && otherNode.y == node.y) ||
                (Math.Abs(otherNode.y - node.y) <= 1) && otherNode.x == node.x))
            {
                var e = new Edge(otherNode, 1);

                for (var i = 0; i < 4; i++)
                {
                    if (otherNode.y - node.y == dirs[i, 0] && otherNode.x - node.x == dirs[i, 1])
                    {
                        e.direction = i;
                    }
                }

                node.connections.Add(e);
            }
        }
        // Calculate heuristic distance to the end node
        node.totalDistance = Math.Sqrt(Math.Pow(node.x - end.x, 2) + Math.Pow(node.y - end.y, 2));
    }

    FindAllPaths();

    return end.visited && end.minCostToStart != null ? end.minCostToStart.Value : int.MaxValue;
}

void buildPath(List<Node> nodes, Node node)
{
    if (node.nearestToStart == null)
    {
        return;
    }

    nodes.Add(node.nearestToStart);
    buildPath(nodes, node.nearestToStart);
}

void FindAllPaths()
{
    // Initialize the start node
    start.minCostToStart = 0;
    var priorityQueue = new List<Tuple<Node, int>> { new(start, 1) };

    do
    {
        // Sort the priority queue based on the cost and heuristic distance
        priorityQueue = [.. priorityQueue.OrderBy(x => x.Item1.minCostToStart + x.Item1.totalDistance)];
        var nodeWithDirection = priorityQueue.First();
        priorityQueue.Remove(nodeWithDirection);

        // Explore connections from the current node
        foreach (var c in nodeWithDirection.Item1.connections.OrderBy(x => x.Cost))
        {
            var connectedNode = c.ConnectedNode;
            if (connectedNode.visited)
            {
                continue;
            }

            // Calculate the cost to reach the connected node
            var cost = nodeWithDirection.Item1.minCostToStart + c.Cost;
            if (c.direction != nodeWithDirection.Item2)
            {
                cost += 1000;
            }

            // Update the minimum cost to reach the connected node
            if (connectedNode.minCostToStart == null || cost < connectedNode.minCostToStart)
            {
                connectedNode.minCostToStart = cost;
                connectedNode.nearestToStart = nodeWithDirection.Item1;

                var newTuple = new Tuple<Node, int>(connectedNode, c.direction);
                if (!priorityQueue.Contains(newTuple))
                {
                    priorityQueue.Add(newTuple);
                }
            }

            // Mark the current node as visited
            nodeWithDirection.Item1.visited = true;
        }

        // Stop the search if the end node is reached
        if (nodeWithDirection.Item1.end)
        {
            return;
        }
    }
    while (priorityQueue.Count > 0);
}

// Node class representing a point in the grid
internal class Node(int x, int y, bool start, bool end)
{
    public int x = x;
    public int y = y;
    public List<Edge> connections = new List<Edge>();

    public bool start = start;
    public bool end = end;

    public int? minCostToStart;
    public double totalDistance;
    public bool visited;
    public Node? nearestToStart;
}

// Edge class representing a connection between two nodes
class Edge(Node connectedNode, int cost)
{
    public int Cost = cost;
    public Node ConnectedNode = connectedNode;
    public int direction;
}