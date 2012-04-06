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

#if NETFX_CORE
			if( typeof( TViewModel ).GetTypeInfo().GetCustomAttribute<SingletonViewModelAttribute>() != null )
#else
			if( typeof( TViewModel ).GetCustomAttributes( typeof( SingletonViewModelAttribute ), false ).Length > 0 )
#endif
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
#if NETFX_CORE
				.Where( t => !t.GetTypeInfo().IsAbstract && t.GetTypeInfo().GetCustomAttribute<SingletonViewModelAttribute>() == null )
#else
				.Where( t => !t.IsAbstract && t.GetCustomAttributes( typeof( SingletonViewModelAttribute ), false ).Length == 0 )
#endif
				.AsSelf()
				.OwnedByLifetimeScope()
				.PropertiesAutowired();

			builder.RegisterAssemblyTypes( assemblies )
				.AssignableTo<ViewModel>()
#if NETFX_CORE
				.Where( t => !t.GetTypeInfo().IsAbstract && t.GetTypeInfo().GetCustomAttribute<SingletonViewModelAttribute>() != null )
#else
				.Where( t => !t.IsAbstract && t.GetCustomAttributes( typeof( SingletonViewModelAttribute ), false ).Length != 0 )
#endif
				.AsSelf()
				.OwnedByLifetimeScope()
				.PropertiesAutowired()
				.SingleInstance();
		}
	}
}
