using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace Digillect.Mvvm
{
	/// <summary>
	/// Extension methods used to help with Autofac registration.
	/// </summary>
	public static class RegistrationExtensions
	{
		/// <summary>
		/// Registers the view model.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the view model.</typeparam>
		/// <param name="builder">The builder.</param>
		/// <returns>Registration builder to be used in registration process.</returns>
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

		/// <summary>
		/// Registers all view models in specified assemblies taking into consideration <see cref="Digillect.Mvvm.SingletonViewModelAttribute"/>.
		/// </summary>
		/// <param name="builder">The builder to use for registration.</param>
		/// <param name="assemblies">Assemblies to look for view models.</param>
		public static void RegisterViewModels( this ContainerBuilder builder, params Assembly[] assemblies )
		{
			builder.RegisterAssemblyTypes( assemblies )
				.AssignableTo<ViewModel>()
				.Where( t => !t.IsAbstract && t.GetCustomAttributes( typeof( SingletonViewModelAttribute ), false ).Length == 0 )
				.AsSelf()
				.OwnedByLifetimeScope()
				.PropertiesAutowired();

			builder.RegisterAssemblyTypes( assemblies )
				.AssignableTo<ViewModel>()
				.Where( t => !t.IsAbstract && t.GetCustomAttributes( typeof( SingletonViewModelAttribute ), false ).Length != 0 )
				.AsSelf()
				.OwnedByLifetimeScope()
				.PropertiesAutowired()
				.SingleInstance();
		}
	}
}
