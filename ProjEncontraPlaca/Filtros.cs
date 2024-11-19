using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ProjEncontraPlaca
{
    class Filtros
    {
        private static void segmenta8(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, Point ini, List<Point> listaPini, List<Point> listaPfim, Color cor_pintar)
        {
            Point menor = new Point(), maior = new Point(), patual = new Point();
            Queue<Point> fila = new Queue<Point>();
            menor.X = maior.X = ini.X;
            menor.Y = maior.Y = ini.Y;
            fila.Enqueue(ini);
            while (fila.Count != 0)
            {
                patual = fila.Dequeue();
                imageBitmapSrc.SetPixel(patual.X, patual.Y, Color.FromArgb(255, 0, 0));
                imageBitmapDest.SetPixel(patual.X, patual.Y, cor_pintar);

                if (patual.X < menor.X)
                    menor.X = patual.X;
                if (patual.X > maior.X)
                    maior.X = patual.X;
                if (patual.Y < menor.Y)
                    menor.Y = patual.Y;
                if (patual.Y > maior.Y)
                    maior.Y = patual.Y;

                if (patual.X > 0)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X - 1, patual.Y));
                        imageBitmapSrc.SetPixel(patual.X - 1, patual.Y, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.Y > 0)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y - 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X - 1, patual.Y - 1));
                            imageBitmapSrc.SetPixel(patual.X - 1, patual.Y - 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.Y > 0)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X, patual.Y - 1);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X, patual.Y - 1));
                        imageBitmapSrc.SetPixel(patual.X, patual.Y - 1, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.X < imageBitmapSrc.Width - 1)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y - 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X + 1, patual.Y - 1));
                            imageBitmapSrc.SetPixel(patual.X + 1, patual.Y - 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.X < imageBitmapSrc.Width - 1)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X + 1, patual.Y));
                        imageBitmapSrc.SetPixel(patual.X + 1, patual.Y, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.Y < imageBitmapSrc.Height - 1)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y + 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X + 1, patual.Y + 1));
                            imageBitmapSrc.SetPixel(patual.X + 1, patual.Y + 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.Y < imageBitmapSrc.Height - 1)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X, patual.Y + 1);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X, patual.Y + 1));
                        imageBitmapSrc.SetPixel(patual.X, patual.Y + 1, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.X > 0)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y + 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X - 1, patual.Y + 1));
                            imageBitmapSrc.SetPixel(patual.X - 1, patual.Y + 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }

            }
            
            if (menor.X > 0)
                menor.X--;
            if (maior.X < imageBitmapSrc.Width - 1)
                maior.X++;
            if (menor.Y > 0)
                menor.Y--;
            if (maior.Y < imageBitmapSrc.Height - 1)
                maior.Y++;
            desenhaRetangulo(imageBitmapDest, menor, maior, Color.FromArgb(255, 0, 0));
            listaPini.Add(menor);
            listaPfim.Add(maior);
        }

        public static void segmentar8conectado(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, List<Point> listaPini, List<Point> listaPfim)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;

                    if (r == 0)
                        segmenta8(imageBitmapSrc, imageBitmapDest, new Point(x, y), listaPini, listaPfim, Color.FromArgb(100, 100, 100));
                }
            }
        }

        private static void desenhaRetangulo(Bitmap imageBitmapDest, Point menor, Point maior, Color cor)
        {
            for (int x = menor.X; x <= maior.X; x++)
            {
                imageBitmapDest.SetPixel(x, menor.Y, cor);
                imageBitmapDest.SetPixel(x, maior.Y, cor);
            }
            for (int y = menor.Y; y <= maior.Y; y++)
            {
                imageBitmapDest.SetPixel(menor.X, y, cor);
                imageBitmapDest.SetPixel(maior.X, y, cor);
            }
        }


        public static Bitmap encontra_placa(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            List<Point> listaPini = new List<Point>();
            List<Point> listaPfim = new List<Point>();
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int recorteLargura = 220;
            int recorteAltura = 50;
            bool encontrouCaracteres = false;
            Bitmap recorteBitmap = null;
            for (int y = 0; y <= height - recorteAltura && !encontrouCaracteres; y=y+5)
            {
                for (int x = 0; x <= width - recorteLargura && !encontrouCaracteres; x=x+10)
                {
                    Rectangle recorte = new Rectangle(x, y, recorteLargura, recorteAltura);
                    recorteBitmap = imageBitmapSrc.Clone(recorte, imageBitmapSrc.PixelFormat);

                    Otsu otsu = new Otsu();
                    otsu.Convert2GrayScaleFast(recorteBitmap);
                    int otsuThreshold = otsu.getOtsuThreshold((Bitmap)recorteBitmap);
                    otsu.threshold(recorteBitmap, otsuThreshold);

                    //string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    //path = Path.Combine(path, "placa");
                    //string pathImageOtsu = Path.Combine(path, "placa" + x + y + ".jpg");
                    //Bitmap imageBitmapSave = (Bitmap)recorteBitmap.Clone();
                    //imageBitmapSave.Save(pathImageOtsu, ImageFormat.Jpeg);

                    Bitmap bitmapAux = new Bitmap(recorteBitmap.Width, recorteBitmap.Height);

                    Bitmap imageBitmap = (Bitmap)recorteBitmap.Clone();
                    Filtros.segmentar8conectado(imageBitmap, bitmapAux, listaPini, listaPfim);

                    int altura, largura;
                    List<Point> _listaPini = new List<Point>();
                    List<Point> _listaPfim = new List<Point>();
                    for (int i = 0; i < listaPini.Count; i++)
                    {
                        altura = listaPfim[i].Y - listaPini[i].Y;
                        largura = listaPfim[i].X - listaPini[i].X;

                        if (altura > 10 && altura < 32 && largura > 3 && largura < 40)
                        {
                            _listaPini.Add(listaPini[i]);
                            _listaPfim.Add(listaPfim[i]);
                        }
                    }
                    if (_listaPini.Count > 6 && estaoNaMesmaLinha(_listaPini, _listaPfim))
                    {
                        if (estaPertoDaBordaDireita(x, y, _listaPfim, recorteLargura))
                        {
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            string pathImageCaractere = Path.Combine(path, "placaTentativaHor.jpg");
                            Bitmap imageBitmapCaractere = (Bitmap)recorteBitmap.Clone();
                            recorteBitmap.Save(pathImageCaractere, ImageFormat.Jpeg);
                        }
                        if (estaPertoDaBordaInferior(x, y, _listaPfim, recorteAltura))
                        {
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            string pathImageCaractere = Path.Combine(path, "placaTentativaVer.jpg");
                            Bitmap imageBitmapCaractere = (Bitmap)recorteBitmap.Clone();
                            recorteBitmap.Save(pathImageCaractere, ImageFormat.Jpeg);
                        }
                        if(!estaPertoDaBordaDireita(x, y, _listaPfim, recorteLargura) && !estaPertoDaBordaInferior(x, y, _listaPfim, recorteAltura))
                        {
                            encontrouCaracteres = true;
                            for (int i = 0; i < _listaPini.Count; i++)
                            {
                                Bitmap aux = (Bitmap)recorteBitmap.Clone();
                                Filtros.desenhaRetangulo(recorteBitmap, _listaPini[i], _listaPfim[i], Color.FromArgb(0, 255, 0));

                                altura = _listaPfim[i].Y - _listaPini[i].Y;
                                largura = _listaPfim[i].X - _listaPini[i].X;

                                Rectangle recorteCaractere = new Rectangle(_listaPini[i].X, _listaPini[i].Y, largura, altura);
                                Bitmap recorteCaractereBitmap = aux.Clone(recorteCaractere, aux.PixelFormat);

                                ClassificacaoCaracteres cl_numeros = new ClassificacaoCaracteres(30, 40, 1, 'S');
                                ClassificacaoCaracteres cl_letras = new ClassificacaoCaracteres(30, 40, 2, 'S');

                                //string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                //string pathImageCaractere = Path.Combine(path, "placa"+i+".jpg");
                                //Bitmap imageBitmapCaractere = (Bitmap)recorteBitmap.Clone();
                                //recorteBitmap.Save(pathImageCaractere, ImageFormat.Jpeg);

                                String transicao;

                                if (i < 3)
                                {
                                    transicao = cl_letras.retornaTransicaoHorizontal(recorteCaractereBitmap);
                                    Console.WriteLine(cl_letras.reconheceCaractereTransicao_2pixels(transicao));
                                }
                                else
                                {
                                    transicao = cl_numeros.retornaTransicaoHorizontal(recorteCaractereBitmap);
                                    Console.WriteLine(cl_numeros.reconheceCaractereTransicao_2pixels(transicao));
                                }
                            }
                            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            string pathImageCaractere = Path.Combine(path, "placa.jpg");
                            Bitmap imageBitmapCaractere = (Bitmap)recorteBitmap.Clone();
                            recorteBitmap.Save(pathImageCaractere, ImageFormat.Jpeg);
                        }
                    }
                    listaPfim.Clear();
                    listaPini.Clear();
                }
            }
            return recorteBitmap;
        }

        private static bool estaPertoDaBordaInferior(int x, int y, List<Point> listaPfim, int recorteAltura)
        {
            int maiorY = 0;
            for (int i = 0; i < listaPfim.Count; i++)
            {
                if (listaPfim[i].Y > maiorY)
                    maiorY = listaPfim[i].Y;
            }
            if (recorteAltura - maiorY < 2)
                return true;
            return false;
        }

        private static bool estaPertoDaBordaDireita(int x, int y, List<Point> listaPfim, int recorteLargura)
        {
            int maiorX = 0;
            for (int i = 0; i < listaPfim.Count; i++)
            {
                if (listaPfim[i].X > maiorX)
                    maiorX = listaPfim[i].X;
            }
            if (recorteLargura - maiorX < 2)
                return true;
            return false;
        }

        private static bool estaoNaMesmaLinha(List<Point> listaPini, List<Point> listaPfim)
        {
            int y = listaPini[0].Y;
            for (int i = 1; i < listaPini.Count; i++)
            {
                if (Math.Abs(y - listaPini[i].Y) > 10)
                    return false;
            }
            return true;
        }
        //-----

        //sem acesso direto a memoria
        public static void threshold(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.1140 + g * 0.5870 + b * 0.2990);
                    if (gs > 127)
                        gs = 255;
                    else
                        gs = 0;

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        public static void countour(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int x, y, x2, y2, aux_cR;
            bool flag;

            Color corB = Color.FromArgb(255, 255, 255);
            for (y = 0; y < height; y++)
                for (x = 0; x < width; x++)
                    imageBitmapDest.SetPixel(x, y, corB);

            Bitmap imageBranca = (Bitmap)imageBitmapDest.Clone();

            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width - 1; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    Color cor2 = imageBitmapSrc.GetPixel(x + 1, y);
                    if (cor.R == 255 && cor2.R == 0 && imageBitmapDest.GetPixel(x + 1, y).R == 255)
                    {
                        Bitmap imageAux = (Bitmap)imageBranca.Clone();
                        x2 = x;
                        y2 = y;
                        do
                        {
                            Color p0, p1, p2, p3, p4, p5, p6, p7;
                            imageBitmapDest.SetPixel(x2, y2, Color.FromArgb(0, 0, 0));
                            imageAux.SetPixel(x2, y2, Color.FromArgb(1, 1, 1));

                            p0 = imageBitmapSrc.GetPixel(x2 + 1, y2);
                            p1 = imageBitmapSrc.GetPixel(x2 + 1, y2 - 1);
                            p2 = imageBitmapSrc.GetPixel(x2, y2 - 1);
                            p3 = imageBitmapSrc.GetPixel(x2 - 1, y2 - 1);
                            p4 = imageBitmapSrc.GetPixel(x2 - 1, y2);
                            p5 = imageBitmapSrc.GetPixel(x2 - 1, y2 + 1);
                            p6 = imageBitmapSrc.GetPixel(x2, y2 + 1);
                            p7 = imageBitmapSrc.GetPixel(x2 + 1, y2 + 1);

                            if (p1.R == 255 && p0.R == 255 && p2.R == 0)
                            {
                                x2 = x2 + 1;
                                y2 = y2 - 1;
                            }
                            else
                             if (p3.R == 255 && p4.R == 0 && p2.R == 255)
                            {
                                x2 = x2 - 1;
                                y2 = y2 - 1;
                            }
                            else
                             if (p5.R == 255 && p4.R == 255 && p6.R == 0)
                            {
                                x2 = x2 - 1;
                                y2 = y2 + 1;
                            }
                            else
                            if (p7.R == 255 && p6.R == 255 && p0.R == 0)
                            {
                                x2 = x2 + 1;
                                y2 = y2 + 1;
                            }
                            else
                            if (p0.R == 255 && p2.R == 0 && p1.R == 0 && imageAux.GetPixel(x2 + 1, y2).R != 2)
                            {
                                x2 = x2 + 1;
                                flag = true;
                                do
                                {
                                    p0 = imageBitmapSrc.GetPixel(x2 + 1, y2);
                                    p1 = imageBitmapSrc.GetPixel(x2 + 1, y2 - 1);
                                    p2 = imageBitmapSrc.GetPixel(x2, y2 - 1);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p0.R == 255 && p2.R == 0 && p1.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        x2 = x2 + 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p0.R == 255 && p2.R == 0 && p1.R == 0 && imageAux.GetPixel(x2 + 1, y2).R == 2)
                                x2 = x2 + 1;
                            else
                            if (p2.R == 255 && p4.R == 0 && p3.R == 0 && imageAux.GetPixel(x2, y2 - 1).R != 2)
                            {
                                y2 = y2 - 1;
                                flag = true;
                                do
                                {
                                    p2 = imageBitmapSrc.GetPixel(x2, y2 - 1);
                                    p3 = imageBitmapSrc.GetPixel(x2 - 1, y2 - 1);
                                    p4 = imageBitmapSrc.GetPixel(x2 - 1, y2);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p2.R == 255 && p4.R == 0 && p3.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        y2 = y2 - 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p2.R == 255 && p4.R == 0 && p3.R == 0 && imageAux.GetPixel(x2, y2 - 1).R == 2)
                                y2 = y2 - 1;
                            else
                            if (p4.R == 255 && p5.R == 0 && p6.R == 0 && imageAux.GetPixel(x2 - 1, y2).R != 2)
                            {
                                x2 = x2 - 1;
                                flag = true;
                                do
                                {
                                    p4 = imageBitmapSrc.GetPixel(x2 - 1, y2);
                                    p5 = imageBitmapSrc.GetPixel(x2 - 1, y2 + 1);
                                    p6 = imageBitmapSrc.GetPixel(x2, y2 + 1);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p4.R == 255 && p5.R == 0 && p6.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        x2 = x2 - 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p4.R == 255 && p5.R == 0 && p6.R == 0 && imageAux.GetPixel(x2 - 1, y2).R == 2)
                                x2 = x2 - 1;
                            else
                            if (p6.R == 255 && p0.R == 0 && p7.R == 0 && imageAux.GetPixel(x2, y2 + 1).R != 2)
                            {
                                y2 = y2 + 1;
                                flag = true;
                                do
                                {
                                    p0 = imageBitmapSrc.GetPixel(x2 + 1, y2);
                                    p6 = imageBitmapSrc.GetPixel(x2, y2 + 1);
                                    p7 = imageBitmapSrc.GetPixel(x2 + 1, y2 + 1);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p6.R == 255 && p0.R == 0 && p7.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        y2 = y2 + 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p6.R == 255 && p0.R == 0 && p7.R == 0 && imageAux.GetPixel(x2, y2 + 1).R == 2)
                                y2 = y2 + 1;
                        }
                        while (x != x2 || y != y2);
                    }
                }
            }
        }





        public static void brancoPreto(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                    if (gs > 220)
                        gs = 255;
                    else
                        gs = 0;

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }
    }
}
