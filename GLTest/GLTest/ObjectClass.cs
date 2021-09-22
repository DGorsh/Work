using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLTest
{
    class ObjectClass
    {
        private float[] _VertexPos;
        private uint[] _PolyInd;

        //конструктор
        public ObjectClass()
        {
            _VertexPos = new float[]
            {
                0.25f, -0.25f, -0.25f,
                0.25f, -0.25f, 0.25f,
                -0.25f, -0.25f, 0.25f,
                -0.25f, -0.25f, -0.25f,
                0.25f, 0.25f, -0.25f,
                0.25f, 0.25f, 0.25f,
                -0.25f, 0.25f, 0.25f,
                -0.25f, 0.25f, -0.25f
            };

            _PolyInd = new uint[]
            {
                4, 0, 3,
                4, 3, 7,
                2, 6, 7,
                2, 7, 3,
                1, 5, 2,
                5, 6, 2,
                0, 4, 1,
                4, 5, 1,
                4, 7, 5,
                7, 6, 5,
                0, 1, 2,
                0, 2, 3
            };

        }

        //геттер - позиция вершин
        public float[] mVertexPos
        {
            get
            {
                return _VertexPos;
            }
        }

        //геттер - индексы вершин
        public uint[] mPolyInd
        {
            get
            {
                return _PolyInd;
            }
        }

        //размер массива с позициями вершин
        public int mVertexSize()
        {
            return (_VertexPos.Length * sizeof(float));
        }

        //размер массива с индексами вершин
        public int mPolySize()
        {
            return (_PolyInd.Length * sizeof(uint));
        }
    }
}
