using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework.Domain
{
    public static class SpecificationExtension
    {
        public static ISpecification<T> And<T>(this ISpecification<T> leftSide, ISpecification<T> rightSide) where T : class
        {
            return new AndSpecification<T>(leftSide, rightSide);
        }
        public static ISpecification<T> Or<T>(this ISpecification<T> leftSide, ISpecification<T> rightSide) where T : class
        {
            return new OrSpecification<T>(leftSide, rightSide);
        }
        public static ISpecification<T> Not<T>(this ISpecification<T> originalSpecification) where T : class
        {
            return new NotSpecification<T>(originalSpecification);
        }
    }
}
