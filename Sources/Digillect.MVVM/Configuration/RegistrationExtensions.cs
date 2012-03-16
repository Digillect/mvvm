using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace Digillect.Mvvm
{
	public static class RegistrationExtensions
	{
		public static IRegistrationBuilder<TViewModel, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterViewModel<TViewModel>( this ContainerBuilder builder )
			where TViewModel : ViewModel
		{
			var rb = builder.RegisterType<TViewModel>();

			if( typeof( TViewModel ).GetCustomAttributes( typeof( SingletonViewModelAttribute ), false ).Length > 0 )
			{
				rb = rb.SingleInstance();
			}

			return rb;
		}

		public static void RegisterViewModels( this ContainerBuilder builder, params Assembly[] assemblies )
		{
			builder.RegisterAssemblyTypes( assemblies )
				.AssignableTo<ViewModel>()
				.Where( t => !t.IsAbstract && t.GetCustomAttributes( typeof( SingletonViewModelAttribute ), false ).Length == 0 )
				.AsSelf();

			builder.RegisterAssemblyTypes( assemblies )
				.AssignableTo<ViewModel>()
				.Where( t => !t.IsAbstract && t.GetCustomAttributes( typeof( SingletonViewModelAttribute ), false ).Length != 0 )
				.AsSelf()
				.SingleInstance();
		}
	}
}
