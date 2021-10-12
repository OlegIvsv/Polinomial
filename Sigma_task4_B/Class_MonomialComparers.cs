using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_task4_B
{
    public class MonomialComparerByDegree : Comparer<Monomial>
    {
        public override int Compare(Monomial x, Monomial y) =>
             x.M > y.M ? 1 : (x.M < y.M ? -1 : 0);
    }
}
