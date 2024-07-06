using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zuma
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Bitmap bg = new Bitmap("bg.jpg");
        Bitmap frog = new Bitmap("frogt.png");
        Timer t = new Timer();
        Random r = new Random();
        Bitmap bball = new Bitmap("bball.png");
        Bitmap rball = new Bitmap("rball.png");
        Bitmap gball = new Bitmap("gball.jpg");
        ball b = new ball();
        float xs, ys, xe, ye;
        float dx=1, dy=1, cx, cy;
        int f = 0, freeze = 0;
        float nmove, move, d;
        int speed = 10, nxtb = 0;
        bool travel = false;

        List<LineSegment> ListOfLines = new List<LineSegment>();
        float xRef, yRef;
        float xsf, ysf, xef, yef, dxf, dyf, mf, m;

        enum Modes { CTRL_POINTS, DRAG };
        BezierCurve obj = new BezierCurve();
        BezierCurve obj2 = new BezierCurve();
        BezierCurve obj3 = new BezierCurve();
        float my_t_inForm = 0.5f;//Ball
        PointF carPoint;
        int mode = 0, freezl = 1;

        int numOfCtrlPoints = 0;
        Modes CurrentMouseMode = Modes.CTRL_POINTS;
        int indexCurrDragNode = -1;
        float t_inc = 0.001f;
        int minc = 3;
        List<ball> mballs = new List<ball>();
        int px = 0;
        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseUp += Form1_MouseUp;
            t.Tick += T_Tick;
            t.Start();
            this.Width=bg.Width;
            this.Height = bg.Height;
            t.Interval = 1;
            xs = 425;
            ys = 360;
            cx = xs;
            cy = ys;
            f = 0;
            travel = false;
            b.type = r.Next(0, 3);
            ball tmp = new ball();
            tmp.type = r.Next(0, 3);
            mballs.Add(tmp);
            obj.loc = 0;
            obj2.loc = 1;
            obj3.loc = 2;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (CurrentMouseMode == Modes.DRAG)
            {
                indexCurrDragNode = -1;
                Drawdoublebuffer(this.CreateGraphics());
            }
            this.Text = e.X + " ; " + e.Y;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentMouseMode == Modes.DRAG && indexCurrDragNode != -1)
            {
                obj3.ModifyCtrlPoint(indexCurrDragNode, e.X, e.Y);
                Drawdoublebuffer(this.CreateGraphics());
            }
            if (ListOfLines.Count > 3)
            {
                xsf = ListOfLines[1].ptS.X;
                ysf = ListOfLines[1].ptS.Y;
                xef = ListOfLines[2].ptS.X;
                yef = ListOfLines[2].ptS.Y;

                dyf = yef - ysf;
                dxf = xef - xsf;
                mf = dyf / dxf;
                m = dy / dx;
                if (px < e.X)
                {
                    for (int i = 0; i < ListOfLines.Count; i++)
                    {
                        LineSegment ptrav = ListOfLines[i];

                        ptrav.ptS.X -= xRef;
                        ptrav.ptS.Y -= yRef;
                        ptrav.ptE.X -= xRef;
                        ptrav.ptE.Y -= yRef;

                        double xn = ptrav.ptS.X * Math.Cos(0.1f) - ptrav.ptS.Y * Math.Sin(0.1f);
                        double Yn = ptrav.ptS.X * Math.Sin(0.1f) + ptrav.ptS.Y * Math.Cos(0.1f);

                        ptrav.ptS.X = (float)xn;
                        ptrav.ptS.Y = (float)Yn;

                        xn = ptrav.ptE.X * Math.Cos(0.1f) - ptrav.ptE.Y * Math.Sin(0.1f);
                        Yn = ptrav.ptE.X * Math.Sin(0.1f) + ptrav.ptE.Y * Math.Cos(0.1f);

                        ptrav.ptE.X = (float)xn;
                        ptrav.ptE.Y = (float)Yn;

                        ptrav.ptS.X += xRef;
                        ptrav.ptS.Y += yRef;
                        ptrav.ptE.X += xRef;
                        ptrav.ptE.Y += yRef;
                    }
                }
                else
                {
                    for (int i = 0; i < ListOfLines.Count; i++)
                    {
                        LineSegment ptrav = ListOfLines[i];

                        ptrav.ptS.X -= xRef;
                        ptrav.ptS.Y -= yRef;
                        ptrav.ptE.X -= xRef;
                        ptrav.ptE.Y -= yRef;

                        double xn = ptrav.ptS.X * Math.Cos(-0.1f) - ptrav.ptS.Y * Math.Sin(-0.1f);
                        double Yn = ptrav.ptS.X * Math.Sin(-0.1f) + ptrav.ptS.Y * Math.Cos(-0.1f);

                        ptrav.ptS.X = (float)xn;
                        ptrav.ptS.Y = (float)Yn;

                        xn = ptrav.ptE.X * Math.Cos(-0.1f) - ptrav.ptE.Y * Math.Sin(-0.1f);
                        Yn = ptrav.ptE.X * Math.Sin(-0.1f) + ptrav.ptE.Y * Math.Cos(-0.1f);

                        ptrav.ptE.X = (float)xn;
                        ptrav.ptE.Y = (float)Yn;

                        ptrav.ptS.X += xRef;
                        ptrav.ptS.Y += yRef;
                        ptrav.ptE.X += xRef;
                        ptrav.ptE.Y += yRef;
                    }
                }
                px = e.X;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            this.Text = e.X + " ; " + e.Y;

            if (f == 1)
            {
                switch (CurrentMouseMode)
                {
                    case Modes.CTRL_POINTS:
                        //if (count < 5)
                        {
                            obj3.SetControlPoint(new Point(e.X, e.Y));
                            numOfCtrlPoints++;
                        }
                        //else
                        {
                            //obj2.SetControlPoint(new Point(e.X, e.Y));
                        }
                        //count++;
                        break;

                    case Modes.DRAG:
                        indexCurrDragNode = obj3.isCtrlPoint(e.X, e.Y);
                        break;
                }
            }

            if (travel == false)
            {
                xe = e.X;
                ye = e.Y;
                dy = ye - ys;
                dx = xe - xs;
                travel = true;

                if (Math.Abs(dx) > Math.Abs(dy))
                {
                    if (xs < xe)
                    {
                        if (ys < ye)
                        {
                            nmove = 2 * dy;
                            move = (2 * dy) - (2 * dx);
                            d = (2 * dy) - dx;
                        }
                        else
                        {
                            nmove = -2 * dy;
                            move = (-2 * dy) - (2 * dx);
                            d = (-2 * dy) - dx;
                        }
                    }
                    else
                    {
                        if (ys < ye)
                        {
                            nmove = 2 * dy;
                            move = (2 * dy) + (2 * dx);
                            d = (2 * dy) + dx;
                        }
                        else
                        {
                            nmove = -2 * dy;
                            move = (-2 * dy) - (2 * dx);
                            d = (-2 * dy) + dx;
                        }
                    }
                }
                else
                {
                    if (xs < xe)
                    {
                        if (ys < ye)
                        {
                            nmove = 2 * dx;
                            move = (2 * dx) - (2 * dy);
                            d = (2 * dx) - dy;
                        }
                        else
                        {
                            nmove = -2 * dx;
                            move = (-2 * dx) - (2 * dy);
                            d = (-2 * dx) - dy;
                        }
                    }
                    else
                    {
                        if (ys < ye)
                        {
                            nmove = 2 * dx;
                            move = (2 * dx) + (2 * dy);
                            d = (2 * dx) + dy;
                        }
                        else
                        {
                            nmove = -2 * dx;
                            move = (2 * dx) - (2 * dy);
                            d = (-2 * dx) + dy;
                        }
                    }
                }
            }

        }

        private void T_Tick(object sender, EventArgs e)
        {

            if (travel)
            {
                if (Math.Abs(dx) > Math.Abs(dy))
                {
                    if (xs < xe)
                    {
                        if (ys > ye)
                        {
                            cx += speed;
                            if (d < 0)
                            {
                                d += nmove;
                            }
                            else
                            {
                                d += move;
                                cy -= speed;
                            }
                        }
                        else
                        {
                            cx += speed;
                            if (d < 0)
                            {
                                d += nmove;
                            }
                            else
                            {
                                d += move;
                                cy += speed;
                            }

                        }
                    }
                    else
                    {
                        if (ys > ye)
                        {
                            cx -= speed;
                            if (d < 0)
                            {
                                d += nmove;
                            }
                            else
                            {
                                d -= move;
                                cy -= speed;
                            }
                        }
                        else
                        {
                            cx -= speed;
                            if (d < 0)
                            {
                                d += nmove;
                            }
                            else
                            {
                                d += move;
                                cy += speed;
                            }

                        }
                    }
                }
                else
                {
                    if (xs < xe)
                    {
                        if (ys > ye)
                        {
                            cy -= speed;
                            if (d < 0)
                            {
                                d -= nmove;
                            }
                            else
                            {
                                d -= move;
                                cx += speed;
                            }
                        }
                        else
                        {
                            cy += speed;
                            if (d < 0)
                            {
                                d += nmove;
                            }
                            else
                            {
                                d += move;
                                cx += speed;
                            }
                        }
                    }
                    else
                    {
                        if (ys > ye)
                        {
                            cy -= speed;
                            if (d < 0)
                            {
                                d += nmove;
                            }
                            else
                            {
                                d -= move;
                                cx -= speed;
                            }
                        }
                        else
                        {
                            cy += speed;
                            if (d < 0)
                            {
                                d -= nmove;
                            }
                            else
                            {
                                d -= move;
                                cx -= speed;
                            }
                        }
                    }
                }

            }

            if (cx < 0 || cx > bg.Width || cy < 0 || cy > bg.Height)
            {
                travel = false;
                xs = 425;
                ys = 360;
                cx = xs;
                cy = ys;
                b.type = r.Next(0, 3);
            }

            freeze++;
            for (int q = 0; q < freezl; q++)
            {
                nxtb++;
                int s = 0;
                int e1 = mballs.Count;
                int r1 = 0;
                if (nxtb == 15)
                {
                    nxtb = 0;
                    ball tmp = new ball();
                    tmp.type = r.Next(0, 3);
                    mballs.Insert(0, tmp);
                }

                for (int i = 0; i < mballs.Count; i++)
                {
                    if (mballs[i].type == 5)
                    {
                        s = 1;
                    }
                    if (s == 0)
                        mballs[i].tp += minc * t_inc;

                    if (mballs[i].tp > 1.0)
                    {
                        mballs[i].curve++;
                        mballs[i].tp = 0.0f;
                        if (mballs[i].curve > 2)
                        {
                            mballs.RemoveAt(i);
                        }
                    }
                    else
                    {
                        PointF curvePoint;
                        curvePoint = obj2.CalcCurvePointAtTime(mballs[i].tp);
                        if (mballs[i].curve == 0)
                            curvePoint = obj.CalcCurvePointAtTime(mballs[i].tp);
                        if (mballs[i].curve == 1)
                            curvePoint = obj2.CalcCurvePointAtTime(mballs[i].tp);
                        if (mballs[i].curve == 2)
                            curvePoint = obj3.CalcCurvePointAtTime(mballs[i].tp);
                        if (cx < curvePoint.X + 50 && cx > curvePoint.X - 4 && cy < curvePoint.Y + 50 && cy > curvePoint.Y - 4)
                        {

                            if (mballs[i].type == b.type)
                            {
                                for (int j = i; j < mballs.Count; j++)
                                {
                                    if (mballs[j].type == b.type)
                                    {
                                        mballs[j].type = 5;
                                    }
                                    else
                                    {
                                        if (mballs[j].type != 5)
                                        {
                                            break;
                                        }
                                    }
                                }
                                for (int j = i; j > 0; j--)
                                {
                                    if (mballs[j].type == b.type)
                                    {
                                        mballs[j].type = 5;
                                    }
                                    else
                                    {
                                        if (mballs[j].type != 5)
                                        {
                                            break;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                int z = 4, tm = 0;
                                for (int j = i; j < mballs.Count; j++)
                                {
                                    if (mballs[j].type != 5)
                                    {
                                        if (z == 4)
                                        {
                                            z = mballs[j].type;
                                        }
                                        else
                                        {
                                            tm = z;
                                            z = mballs[j].type;
                                            mballs[j].type = tm;
                                        }
                                    }
                                }
                                mballs[i].type = b.type;
                            }

                            travel = false;
                            xs = 425;
                            ys = 360;
                            cx = xs;
                            cy = ys;
                            b.type = r.Next(0, 3);

                        }

                    }
                    if (i + 1 < mballs.Count)
                    {
                        if (mballs[i].tp == mballs[i+1].tp && mballs[i].curve == mballs[i+1].curve && mballs[i+1].type == 5)
                        {
                            mballs.RemoveAt(i+1);
                            if (mballs[i].type == mballs[i + 1].type)
                            {
                                for (int j = i; j < mballs.Count; j++)
                                {
                                    if (mballs[j].type == mballs[i + 1].type)
                                    {
                                        mballs[j].type = 5;
                                    }
                                    else
                                    {
                                        if (mballs[j].type != 5)
                                        {
                                            break;
                                        }
                                    }
                                }
                                for (int j = i; j > 0; j--)
                                {
                                    if (mballs[j].type == mballs[i + 1].type)
                                    {
                                        mballs[j].type = 5;
                                    }
                                    else
                                    {
                                        if (mballs[j].type != 5)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }


            Drawdoublebuffer(this.CreateGraphics());

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                mode++;
            if (e.KeyCode == Keys.Down)
                mode--;


            if (mode == 1)
            {
                if (e.KeyCode == Keys.Left)
                    speed -= 5;

                if (e.KeyCode == Keys.Right)
                    speed += 5;

            }

            if (mode == 2)
            {
                if (e.KeyCode == Keys.Left)
                {
                    f = 1;
                    CurrentMouseMode = Modes.CTRL_POINTS;
                }
                if (e.KeyCode == Keys.Right)
                {
                    CurrentMouseMode = Modes.DRAG;
                }
            }

            if (mode == 3)
            {
                if (e.KeyCode == Keys.Left)
                    freezl --;
                if (e.KeyCode == Keys.Right)
                    freezl ++;

                if (freeze < 0)
                    freeze = 0;
            }


            if (mode < 0)
                mode = 0;
            if (mode > 3)
                mode = 3;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Drawdoublebuffer(e.Graphics);
        }
        void Drawdoublebuffer(Graphics g) //main
        {
            Graphics g2 = Graphics.FromImage(bitmap);
            Drawscene(g2);
            g.DrawImage(bitmap, 0, 0);
        }
        void Drawscene(Graphics g)
        {
            g.Clear(Color.Black);
            g.DrawImage(bg,0, 0);
            g.DrawImage(frog, bg.Width/2-135, bg.Height/2-120);
            if (b.type == 0)
            {
                g.DrawImage(bball, cx - 5, cy - 5);
            }
            if (b.type == 1)
            {
                g.DrawImage(rball, cx - 5, cy - 5);
            }
            if (b.type == 2)
            {
                g.DrawImage(gball, cx - 5, cy - 5);
            }


            obj.DrawCurve(g, mballs);
            obj2.DrawCurve(g, mballs);
            obj3.DrawCurve(g, mballs);
            carPoint = obj.CalcCurvePointAtTime(my_t_inForm);
            if (mode == 1)
            {
                g.DrawString("MODE : speed", new Font("System", 15), Brushes.Black, 10, 10);
            }
            if (mode == 2)
            {
                g.DrawString("MODE : points", new Font("System", 15), Brushes.Black, 10, 10);
            }
            if (mode == 3)
            {
                g.DrawString("MODE : freeze", new Font("System", 15), Brushes.Black, 10, 10);
            }

            for (int i = 0; i < ListOfLines.Count; i++)
            {
                LineSegment ptrav = ListOfLines[i];
                ptrav.DrawYourSelf(g);
            }

            g.FillEllipse(Brushes.Orange, xRef - 5, yRef - 5, 10, 10);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(this.Width, this.Height);

            obj.SetControlPoint(new Point(95,675));
            obj.SetControlPoint(new Point(15,320));
            obj.SetControlPoint(new Point(-60,-65));
            obj.SetControlPoint(new Point(345,-35));
            obj.SetControlPoint(new Point(1015, 45));
            obj.SetControlPoint(new Point(825, 280));

            obj2.SetControlPoint(new Point(825, 280));
            obj2.SetControlPoint(new Point(780, 365));
            obj2.SetControlPoint(new Point(950, 458));
            obj2.SetControlPoint(new Point(760, 760));
            obj2.SetControlPoint(new Point(170, 605));

            obj3.SetControlPoint(new Point(170, 605));
            obj3.SetControlPoint(new Point(200, 420));
            obj3.SetControlPoint(new Point(440, 555));
            obj3.SetControlPoint(new Point(910, 580));
            obj3.SetControlPoint(new Point(680, 325));

            int Xs = 405;
            int Ys = 280;
            int[] X = { Xs, Xs + 100, Xs + 100, Xs, Xs };
            int[] Y = { Ys, Ys, Ys + 100, Ys + 100, Ys };

            xRef = 455;
            yRef = 330;
            for (int i = 0; i < 4; i++)
            {
                LineSegment pnn = new LineSegment();
                pnn.ptS = new PointF(X[i], Y[i]);
                pnn.ptE = new PointF(X[i + 1], Y[i + 1]);

                ListOfLines.Add(pnn);
            }
        }


        public class ball
        {
            public int x;
            public int y;
            public int type = 5;
            public float tp = 3 * 0.001f;
            public int curve = 0;
        }

        public class BezierCurve
        {

            public List<Point> ControlPoints;

            public float t_inc = 0.001f;

            public Color cl = Color.Red;
            public Color clr1 = Color.Black;
            public Color ftColor = Color.Black;
            Bitmap bball = new Bitmap("bball.png");
            Bitmap rball = new Bitmap("rball.png");
            Bitmap gball = new Bitmap("gball.jpg");
            public int loc;


            public BezierCurve()
            {
                ControlPoints = new List<Point>();
            }


            public float Factorial(int n)
            {
                float res = 1.0f;

                for (int i = 2; i <= n; i++)
                    res *= i;

                return res;
            }

            public float C(int n, int i)
            {
                float res = Factorial(n) / (Factorial(i) * Factorial(n - i));
                return res;
            }

            public double Calc_B(float t, int i)
            {
                int n = ControlPoints.Count - 1;
                double res = C(n, i) *
                                Math.Pow((1 - t), (n - i)) *
                                Math.Pow(t, i);
                return res;
            }

            public Point GetPoint(int i)
            {
                return ControlPoints[i];
            }

            public PointF CalcCurvePointAtTime(float t)
            {
                PointF pt = new PointF();
                for (int i = 0; i < ControlPoints.Count; i++)
                {
                    float B = (float)Calc_B(t, i);
                    pt.X += B * ControlPoints[i].X;
                    pt.Y += B * ControlPoints[i].Y;
                }

                return pt;
            }

            public void DrawControlPoints(Graphics g)
            {
                Font Ft = new Font("System", 10);
                for (int i = 0; i < ControlPoints.Count; i++)
                {
                    g.FillEllipse(new SolidBrush(clr1),
                                    ControlPoints[i].X - 5,
                                    ControlPoints[i].Y - 5, 10, 10);

                    g.DrawString("P# " + i, Ft, new SolidBrush(ftColor), ControlPoints[i].X - 15, ControlPoints[i].Y - 15);
                }
            }

            public int isCtrlPoint(int XMouse, int YMouse)
            {
                Rectangle rc;
                for (int i = 0; i < ControlPoints.Count; i++)
                {
                    rc = new Rectangle(ControlPoints[i].X - 5, ControlPoints[i].Y - 5, 10, 10);
                    if (XMouse >= rc.Left && XMouse <= rc.Right && YMouse >= rc.Top && YMouse <= rc.Bottom)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public void ModifyCtrlPoint(int i, int XMouse, int YMouse)
            {
                Point p = ControlPoints[i];

                p.X = XMouse;
                p.Y = YMouse;
                ControlPoints[i] = p;
            }

            public void SetControlPoint(Point pt)
            {
                ControlPoints.Add(pt);
            }

            public void DrawCurvePoints(Graphics g)
            {
                if (ControlPoints.Count <= 0)
                    return;

                PointF curvePoint;
                for (float t = 0.0f; t <= 1.0; t += t_inc)
                {
                    curvePoint = CalcCurvePointAtTime(t);
                    
                    g.FillEllipse(new SolidBrush(cl),
                                    curvePoint.X - 4, curvePoint.Y - 4, 8, 8);
                }
            }
            public void MoveCurvePoints(Graphics g, List<ball> mballs)
            {
                if (ControlPoints.Count <= 0)
                    return;
                for (int i = 0; i < mballs.Count; i++) 
                {
                    if (loc == 0 && mballs[i].curve == 0)
                    {
                        PointF curvePoint;
                        curvePoint = CalcCurvePointAtTime(mballs[i].tp);
                        if (mballs[i].type == 0)
                        {
                            g.DrawImage(bball, curvePoint.X - 4, curvePoint.Y - 4);
                        }
                        if (mballs[i].type == 1)
                        {
                            g.DrawImage(rball, curvePoint.X - 4, curvePoint.Y - 4);
                        }
                        if (mballs[i].type == 2)
                        {
                            g.DrawImage(gball, curvePoint.X - 4, curvePoint.Y - 4);
                        }
                    }
                    if (loc == 1 && mballs[i].curve == 1)
                    {
                        PointF curvePoint;
                        curvePoint = CalcCurvePointAtTime(mballs[i].tp);
                        if (mballs[i].type == 0)
                        {
                            g.DrawImage(bball, curvePoint.X - 4, curvePoint.Y - 4);
                        }
                        if (mballs[i].type == 1)
                        {
                            g.DrawImage(rball, curvePoint.X - 4, curvePoint.Y - 4);
                        }
                        if (mballs[i].type == 2)
                        {
                            g.DrawImage(gball, curvePoint.X - 4, curvePoint.Y - 4);
                        }
                    }
                    if (loc == 2 && mballs[i].curve == 2)
                    {
                        PointF curvePoint;
                        curvePoint = CalcCurvePointAtTime(mballs[i].tp);
                        if (mballs[i].type == 0)
                        {
                            g.DrawImage(bball, curvePoint.X - 4, curvePoint.Y - 4);
                        }
                        if (mballs[i].type == 1)
                        {
                            g.DrawImage(rball, curvePoint.X - 4, curvePoint.Y - 4);
                        }
                        if (mballs[i].type == 2)
                        {
                            g.DrawImage(gball, curvePoint.X - 4, curvePoint.Y - 4);
                        }
                    }
                }
                //g.FillEllipse(new SolidBrush(cl),curvePoint.X - 4, curvePoint.Y - 4, 8, 8);
            }

            public void DrawCurve(Graphics g, List<ball> mballs)
            {
                DrawControlPoints(g);
                //DrawCurvePoints(g);
                MoveCurvePoints(g,mballs);
            }
        }

        public class LineSegment
        {
            public PointF ptS, ptE;

            public void DrawYourSelf(Graphics g)
            {
                g.DrawLine(Pens.Yellow, ptS.X, ptS.Y, ptE.X, ptE.Y);
                g.FillEllipse(Brushes.Red, ptS.X - 5, ptS.Y - 5, 10, 10);
                g.FillEllipse(Brushes.Red, ptE.X - 5, ptE.Y - 5, 10, 10);
            }
        }
    }
}

