using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digillect.Mvvm.Tests
{
	public static class TaskSpecificationExtensions
	{
		public static T Await<T>( this Task<T> task )
		{
			try
			{
				task.Wait();
			}
			catch( AggregateException e )
			{
				if( e.InnerExceptions.Count == 1 )
				{
					throw e.InnerExceptions.First();
				}
				throw;
			}

			return task.Result;
		}

		public static void Await( this Task task )
		{
			try
			{
				task.Wait();
			}
			catch( AggregateException e )
			{
				if( e.InnerExceptions.Count == 1 )
				{
					throw e.InnerExceptions.First();
				}
				throw;
			}
		}
	}
}
