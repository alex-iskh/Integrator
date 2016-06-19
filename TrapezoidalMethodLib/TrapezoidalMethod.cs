using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FormulaExecutorLib;

namespace TrapezoidalMethodLib
{
    public class TrapezoidalMethod : Component
    {
        public double Integration(FormulaExecutor integrand, double a, double b, double eps)
        {
            //заменим особые предельные точки на точки из окрестности
            if(Double.IsNaN(integrand.Execute(a))||Double.IsInfinity(integrand.Execute(a)))
                a += 0.0001*eps*(b-a);
            if(Double.IsNaN(integrand.Execute(b))||Double.IsInfinity(integrand.Execute(b)))
                b -= 0.0001*eps*(b-a);
            //посчитаем производную на 100 точках и определим шаг разбиения h для метода
            int n = 1000;
            double[] deriv = new double[n];
            deriv[0] = (integrand.Execute(a+(b-a)/(n-1)) - integrand.Execute(a)) / ((b-a)/(n-1));
            for (int i = 1; i < n; ++i)
            {
                deriv[i] = (integrand.Execute(a+i*(b-a)/(n-1)) - integrand.Execute(a+(i-1)*(b-a)/(n-1))) / ((b-a)/(n-1));
            }
            //по значению производной посчитаем значение второй производной  и определим шаг разбиения h для метода
            double[] secDeriv = new double[n];
            secDeriv[0] = (deriv[1] - deriv[0]) / ((b-a)/(n-1));
            double maxSecDeriv = Math.Abs(secDeriv[0]);
            for (int i = 1; i < n; ++i)
            {
                secDeriv[i] = (deriv[i] - deriv[i-1]) / ((b-a)/(n-1));
                if (Math.Abs(secDeriv[i]) > maxSecDeriv) maxSecDeriv = Math.Abs(secDeriv[i]);
            }
            double h = (b-a)/Math.Ceiling(Math.Sqrt(maxSecDeriv*Math.Pow(b-a, 3)/(12*eps)));
            //найдем значение интеграла
            double sum = integrand.Execute(a) + integrand.Execute(b);
            for (int i = 1; i * h < (b - a); ++i)
                sum += 2*integrand.Execute(a+i*h);
            return 0.5*h*sum;
        }
    }
}
