using System;
using System.Collections;
using System.Collections.Generic;

namespace Dijkstras
{
    class Graph
    {
        Dictionary<char, Dictionary<char, int>> vertices = new Dictionary<char, Dictionary<char, int>>();

        public void add_vertex(char name, Dictionary<char, int> edges)
        {
            vertices[name] = edges;
        }

        public List<char> shortest_path(char start, char finish)
        {
            var previous = new Dictionary<char, char>();
            var distances = new Dictionary<char, int>();
            var nodes = new List<char>();

            List<char> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<char>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }
            //add the start point
            path.Add(start);
            return path;
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            Graph g = new Graph();
            g.add_vertex('A', new Dictionary<char, int>() {{'B', 7}, {'C', 8}});
            g.add_vertex('B', new Dictionary<char, int>() {{'A', 7}, {'F', 2}});
            g.add_vertex('C', new Dictionary<char, int>() {{'A', 8}, {'F', 6}, {'G', 4}});
            g.add_vertex('D', new Dictionary<char, int>() {{'F', 8}});
            g.add_vertex('E', new Dictionary<char, int>() {{'H', 1}});
            g.add_vertex('F', new Dictionary<char, int>() {{'B', 2}, {'C', 6}, {'D', 8}, {'G', 9}, {'H', 3}});
            g.add_vertex('G', new Dictionary<char, int>() {{'C', 4}, {'F', 9}});
            g.add_vertex('H', new Dictionary<char, int>() {{'E', 1}, {'F', 3}});
            var paths = g.shortest_path('A', 'H');
            //use our iteration for out put
            IterationSample itSample = new IterationSample(paths);
            foreach (var value in itSample)
            {
                Console.WriteLine(value);
            }
            // out put is A B F H
            Console.ReadLine();
        }
    }
    #region Our Iterator for --
    class IterationSample : IEnumerable
    {
        public List<char> values;
        public Int32 startingPoint;
        public IterationSample(List<char> values, Int32 startingPoint)
        {
            this.values = values;
            this.startingPoint = startingPoint;
        }
        public IterationSample(List<char> values)
        {
            this.values = values;
            this.startingPoint = 0;
        }
        public IEnumerator GetEnumerator()
        {
            return new IterationSampleEnumerator(this);
        }
    }
    class IterationSampleEnumerator : IEnumerator
    {
        IterationSample parent;
        Int32 position;
        internal IterationSampleEnumerator(IterationSample parent)
        {
            this.parent = parent;
            position = parent.values.Count;
        }

        public bool MoveNext()
        {
            if (position != -1) 
            {
                position--;
            }
            return position >-1;
        }

        public object Current
        {
            get
            {
                if (position == -1 || position == parent.values.Count)
                {
                    throw new InvalidOperationException();
                }
                Int32 index = position + parent.startingPoint;
                index = index % parent.values.Count;
                return parent.values[index];
            }
        }

        public void Reset()
        {
            position = parent.values.Count;
        }
    }
    
    #endregion
}
