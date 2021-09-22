using System;
using Tao.FreeGlut;
using Tao.OpenGl;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace GLTest
{
    class Program
    {
        const int WIDTH = 800, HEIGTH = 400; // ширина и высота окна
        static bool isShader = false; // обводка

        static uint VertexBufferObj; //верш. буфер
        static uint IndexBufferObj; //буфер индексов
        static int DataSize;
        static uint ShaderProgramObj; //шейдер (обычный кубик)
        static uint ShaderProgramOut; //шейдер (для кубика-обводки)

        static void on_display()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            //Viewport 1
            Gl.glViewport(0, 0, WIDTH / 2, HEIGTH);
            Gl.glLoadIdentity();
            Glu.gluLookAt(0.0, 0.0, 0.0, 2.0, 2.0, 2.0, 0.0, 1.0, 0.0);
            viewport_scene();

            //Viewport 2
            Gl.glViewport(WIDTH / 2, 0, WIDTH / 2, HEIGTH);
            Gl.glLoadIdentity();
            Glu.gluLookAt(0.0, 0.0, 0.0, 2.0, 0.0, 0.0, 0.0, 1.0, 0.0);
            viewport_scene();
            

            Glut.glutSwapBuffers();
        }

        static void viewport_scene()
        {
            Gl.glBindBuffer(Gl.GL_ELEMENT_ARRAY_BUFFER, IndexBufferObj);
            if (isShader)
            {
                //обводка
                Gl.glPushMatrix();
                Gl.glScaled(1.1, 1.1, 1.1);
                Gl.glUseProgram(ShaderProgramOut);
                Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, (IntPtr)null);
                Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
                Gl.glDrawElements(Gl.GL_TRIANGLES, DataSize / sizeof(uint), Gl.GL_UNSIGNED_INT, (IntPtr)0);
                Gl.glPopMatrix();
            }

            // Кубик
            Gl.glUseProgram(ShaderProgramObj);
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            Gl.glPushMatrix();
            Gl.glScaled(1.0, 1.0, 1.0);
            Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, (IntPtr)null);
            Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
            Gl.glDrawElements(Gl.GL_TRIANGLES, DataSize / sizeof(uint), Gl.GL_UNSIGNED_INT, (IntPtr)0);
            Gl.glPopMatrix();

            Gl.glDisable(Gl.GL_DEPTH_TEST);

        }

        static void on_SpecialKeys(int ppKey, int ppXPos, int ppYPos)
        {
            //Нажатие на клавишу "Стрелка вверх" включит обводку куба. Повторное нажатие выключит.
            if (ppKey == Glut.GLUT_KEY_UP)
            {
                if (isShader) { isShader = false; } else { isShader = true; }
            }
        }

        static void Main()
        {

            //инициализация окна с помощью glut
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DEPTH | Glut.GLUT_DOUBLE | Glut.GLUT_RGBA);
            Glut.glutInitWindowPosition(100, 100);
            Glut.glutInitWindowSize(WIDTH, HEIGTH);
            Glut.glutCreateWindow("Тестовое задание. Горшков Д.С.");

            //объект класса 3д-объекта (куба)
            ObjectClass CubeObj = new ObjectClass();
            DataSize = CubeObj.mVertexSize();

            //== шейдеры == обычный кубик
            //специально не внесено в отдельную функцию загрузки шейдера - для большей наглядности
            //для окраски был использован простейший фраг. шейдер, вершинный шейдер не использован, его иниц. аналогична фрагм. 

            //фрагментный шейдер
            string ShaderText = mReadFile("FragNormal.glsl");
            uint FragmentShaderObj = (uint)Gl.glCreateShader(Gl.GL_FRAGMENT_SHADER);
            Gl.glShaderSource(FragmentShaderObj, 1, new[] { ShaderText }, new[] { ShaderText.Length });
            Gl.glCompileShader(FragmentShaderObj);

            //общий шейдер
            ShaderProgramObj = (uint)Gl.glCreateProgram();
            Gl.glAttachShader(ShaderProgramObj, FragmentShaderObj);
            Gl.glLinkProgram(ShaderProgramObj);

            //очистка
            Gl.glDeleteShader(FragmentShaderObj);


            //== шейдеры == обводка кубика
            //фрагментный шейдер
            ShaderText = mReadFile("FragOutline.glsl");
            FragmentShaderObj = (uint)Gl.glCreateShader(Gl.GL_FRAGMENT_SHADER);
            Gl.glShaderSource(FragmentShaderObj, 1, new[] { ShaderText }, new[] { ShaderText.Length });
            Gl.glCompileShader(FragmentShaderObj);

            //общий шейдер
            ShaderProgramOut = (uint)Gl.glCreateProgram();
            Gl.glAttachShader(ShaderProgramOut, FragmentShaderObj);
            Gl.glLinkProgram(ShaderProgramOut);

            //очистка
            Gl.glDeleteShader(FragmentShaderObj);

            // == VBO ==
            //инициализация вершинного буфера (VBO)
            Gl.glGenBuffers(1, out VertexBufferObj);
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, VertexBufferObj);
            Gl.glBufferData(Gl.GL_ARRAY_BUFFER, (IntPtr)CubeObj.mVertexSize(), CubeObj.mVertexPos, Gl.GL_STATIC_DRAW);

            //аттрибуты
            //Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 3 * sizeof(float), (IntPtr)0);
            //Gl.glEnableVertexAttribArray(0);


            // == Индексы ==
            //инициализация
            Gl.glGenBuffers(1, out IndexBufferObj);
            Gl.glBindBuffer(Gl.GL_ELEMENT_ARRAY_BUFFER, IndexBufferObj);
            Gl.glBufferData(Gl.GL_ELEMENT_ARRAY_BUFFER, (IntPtr)CubeObj.mPolySize(), CubeObj.mPolyInd, Gl.GL_STATIC_DRAW);
            DataSize = CubeObj.mPolySize();


            //функции для работы окна.
            Glut.glutDisplayFunc(on_display);
            Glut.glutIdleFunc(on_display); 
            Glut.glutSpecialFunc(on_SpecialKeys);
            Glut.glutMainLoop();
        }


        //чтение из файла
        static string mReadFile (string ppFilename)
        {
            StringBuilder OutData = new StringBuilder();
            try
            {
                using (StreamReader File = new StreamReader(ppFilename))
                {
                    string tReaded;
                    while ((tReaded = File.ReadLine()) != null)
                    {
                        OutData.Append(tReaded).Append("\n ");
                    }
                }
            } catch (Exception e)
            {
                return "";
            }
            return OutData.ToString();
        }

        //==================================================================
        // Задание 2
        // Общий случай при наличии готовой камеры.
        // Значения позиции камеры можно передать в качестве аргументов.
        //==================================================================

        static float[] GetCoords(int ppXScreenPos, int ppYScreenPos)
        {
            //подготовка к решению
            //чтение z-буфера в точке
            float[] Depth = new float[1];
            Gl.glReadBuffer(Gl.GL_FRONT);
            Gl.glReadPixels(ppXScreenPos, ppYScreenPos, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, Depth);


            //Depth[0] = 0.25f; - значение для проверки
            //настройки камеры 
            float[] CameraPos = { 2.0f, 2.0f, 2.0f }; // позиция камеры
            float[] CameraLook = { 0.0f, 0.0f, 0.0f }; // направление


            //само решение
            //Расчет трехмерных координат
            //Позиция камеры + вектор направления объектива * глубина
            float[] ResultPoint = new float[3];
            ResultPoint[0] = CameraPos[0] + (CameraLook[0] - CameraPos[0]) * (Depth[0]);
            ResultPoint[1] = CameraPos[1] + (CameraLook[1] - CameraPos[1]) * (Depth[0]);
            ResultPoint[2] = CameraPos[2] + (CameraLook[2] - CameraPos[2]) * (Depth[0]);
            return ResultPoint;
        }
    }
}
