using System;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace GLTest
{
    class Program
    {
        const int WIDTH = 800, HEIGTH = 400; // ширина и высота окна
        static bool isShader = false; // обводка

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
            if (!isShader)
            {
                // Если без обводки
                Gl.glEnable(Gl.GL_DEPTH_TEST);
                Gl.glColor3f(0.5f, 0.5f, 0.0f);
                Glut.glutSolidCube(0.5);
                Gl.glDisable(Gl.GL_DEPTH_TEST);
            }
            else
            {
                // Если с обводкой
                Gl.glDisable(Gl.GL_DEPTH_TEST);
                Gl.glColor3f(0.5f, 0.0f, 0.5f);
                Glut.glutSolidCube(0.6);

                Gl.glEnable(Gl.GL_DEPTH_TEST);
                Gl.glColor3f(0.5f, 0.5f, 0.0f);
                Glut.glutSolidCube(0.5);
                Gl.glDisable(Gl.GL_DEPTH_TEST);
            }

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

            //функции для работы окна.
            Glut.glutDisplayFunc(on_display);
            Glut.glutIdleFunc(on_display); 
            Glut.glutSpecialFunc(on_SpecialKeys);
            Glut.glutMainLoop();
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
