using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DijkstrasAlgorithmPresentation
{
    public class Edge
    {
        public Vertex start { get; set; }
        public ContentPresenter startPresentser => MainWindow.vertexPresenterDictionary[start];
        public Vertex end { get; set; }
        public ContentPresenter endPresentser => MainWindow.vertexPresenterDictionary[end];
        public double weight { get; set; }
    }
}
