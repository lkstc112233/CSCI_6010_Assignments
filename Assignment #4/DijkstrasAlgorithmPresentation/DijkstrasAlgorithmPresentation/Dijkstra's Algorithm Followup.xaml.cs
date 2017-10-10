using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DijkstrasAlgorithmPresentation
{
    /// <summary>
    /// Dijkstra_s_Algorithm_Followup.xaml 的交互逻辑
    /// </summary>
    public partial class Dijkstra_s_Algorithm_Followup_Window : Window
    {
        internal ViewModelVertexEdge viewModel;

        public Dijkstra_s_Algorithm_Followup_Window()
        {
            InitializeComponent();
        }

    }

    public class Heap<T>
    {
        public delegate bool SmallerThan(T t1, T t2);
        private SmallerThan smallerThanMethod;
        public Heap(SmallerThan s)
        {
            smallerThanMethod = s;
            m_maxCapacity = 10;
            m_data = new T[m_maxCapacity];
            m_size = 0;
        }

        private T[] m_data;
        private int m_maxCapacity;
        private int m_size;

        private int getParentIndex(int i)
        {
            if (i == 0)
                return -1;
            return (i + 1) / 2 - 1;
        }
        private bool hasChild(int i)
        {
            return 2 * i + 1 < m_size;
        }
        private int getLeftChildIndex(int i)
        {
            if (2 * i + 1 < m_size)
                return 2 * i + 1;
            return -1;
        }
        private int getRightChildIndex(int i)
        {
            if (2 * i + 2 < m_size)
                return 2 * i + 2;
            return -1;
        }

        public bool IsEmpty()
        {
            return m_size == 0;
        }

        public void Add(T t)
        {
            if (m_size >= m_maxCapacity)
                ExpandCapacity();
            m_data[m_size] = t;
            int checking = m_size;
            m_size += 1;
            while (getParentIndex(checking) >= 0 && smallerThanMethod(m_data[checking], m_data[getParentIndex(checking)]))
            {
                var temp = m_data[checking];
                m_data[checking] = m_data[getParentIndex(checking)];
                m_data[getParentIndex(checking)] = temp;
                checking = getParentIndex(checking);
            }
        }

        public void Update(T t)
        {
            int checking = 0;
            for (int i = 0; i < m_size; ++i)
                if (ReferenceEquals(m_data[i], t))
                {
                    checking = i;
                    break;
                }
            while (getParentIndex(checking) >= 0 && smallerThanMethod(m_data[checking], m_data[getParentIndex(checking)]))
            {
                var temp = m_data[checking];
                m_data[checking] = m_data[getParentIndex(checking)];
                m_data[getParentIndex(checking)] = temp;
                checking = getParentIndex(checking);
            }
        }

        public T GetMin()
        {
            if (IsEmpty())
                return default(T);
            return m_data[0];
        }

        public void RemoveMin()
        {
            if (IsEmpty())
                return;
            var foo = m_data[0];
            m_data[0] = m_data[m_size - 1];
            m_data[m_size - 1] = foo;
            m_size -= 1;
            int checking = 0;
            while (hasChild(checking))
            {
                if (smallerThanMethod(m_data[getLeftChildIndex(checking)], m_data[checking]))
                    if (getRightChildIndex(checking) >= 0)
                        if (smallerThanMethod(m_data[getRightChildIndex(checking)], m_data[checking]))
                            if (smallerThanMethod(m_data[getLeftChildIndex(checking)], m_data[getRightChildIndex(checking)]))
                            {
                                var temp = m_data[checking];
                                m_data[checking] = m_data[getLeftChildIndex(checking)];
                                m_data[getLeftChildIndex(checking)] = temp;
                                checking = getLeftChildIndex(checking);
                            }
                            else
                            {
                                var temp = m_data[checking];
                                m_data[checking] = m_data[getRightChildIndex(checking)];
                                m_data[getRightChildIndex(checking)] = temp;
                                checking = getRightChildIndex(checking);
                            }
                        else
                        {
                            var temp = m_data[checking];
                            m_data[checking] = m_data[getLeftChildIndex(checking)];
                            m_data[getLeftChildIndex(checking)] = temp;
                            checking = getLeftChildIndex(checking);
                        }
                    else
                    {
                        var temp = m_data[checking];
                        m_data[checking] = m_data[getLeftChildIndex(checking)];
                        m_data[getLeftChildIndex(checking)] = temp;
                        checking = getLeftChildIndex(checking);
                    }
                else if (getRightChildIndex(checking) >= 0)
                    if (smallerThanMethod(m_data[getRightChildIndex(checking)], m_data[checking]))
                    {
                        var temp = m_data[checking];
                        m_data[checking] = m_data[getRightChildIndex(checking)];
                        m_data[getRightChildIndex(checking)] = temp;
                        checking = getRightChildIndex(checking);
                    }
                    else
                        break;
                else
                    break;
            }
        }
        
        private void ExpandCapacity()
        {
            int tempSize = m_maxCapacity * 2;
            T[] tempData = new T[tempSize];
            for (int i = 0; i < m_size; ++i)
                tempData[i] = m_data[i];
            m_maxCapacity = tempSize;
            m_data = tempData;
        }
    }
}
