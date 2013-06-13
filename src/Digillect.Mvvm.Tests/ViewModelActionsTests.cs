using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Machine.Specifications;

namespace Digillect.Mvvm.Tests
{
	[Subject( typeof( ViewModel ), "Actions" )]
	public class ViewModelActionsTest
	{
		protected static ActionViewModel sut;
		protected static Session session;

		Establish context = () => sut = new ActionViewModel();

		Because of = () => sut.Load( session ).Await();
	}

	public class when_view_model_registers_action_without_a_name : ViewModelActionsTest
	{
		Establish context = () => session = sut.CreateSession();

		It should_register_default_action = () => sut.P1.ShouldEqual( 1 );
	}

	public class when_view_model_extends_an_action : ViewModelActionsTest
	{
		Establish context = () => session = sut.CreateSession( "ToExtend" );

		It should_execute_original_part = () => sut.P1.ShouldEqual( 1 );
		It should_execute_extension_part = () => sut.P2.ShouldEqual( 2 );
	}

	public class when_parallel_group_contains_nested_parallel_group : ViewModelActionsTest
	{
		Establish context = () => session = sut.CreateSession( "PP" );

		It should_execute_part_1_in_correct_order = () => sut.P1.ShouldEqual( 4 );
		It should_execute_part_2_in_correct_order = () => sut.P2.ShouldEqual( 3 );
		It should_execute_part_3_in_correct_order = () => sut.P3.ShouldEqual( 1 );
		It should_execute_part_4_in_correct_order = () => sut.P4.ShouldEqual( 5 );
		It should_execute_part_5_in_correct_order = () => sut.P5.ShouldEqual( 2 );
	}

	public class when_parallel_group_contains_nested_sequential_group : ViewModelActionsTest
	{
		Establish context = () => session = sut.CreateSession( "PS" );

		It should_execute_part_1_in_correct_order = () => sut.P1.ShouldEqual( 4 );
		It should_execute_part_2_in_correct_order = () => sut.P2.ShouldEqual( 2 );
		It should_execute_part_3_in_correct_order = () => sut.P3.ShouldEqual( 3 );
		It should_execute_part_4_in_correct_order = () => sut.P4.ShouldEqual( 5 );
		It should_execute_part_5_in_correct_order = () => sut.P5.ShouldEqual( 1 );
	}

	public class when_sequential_group_contains_nested_parallel_group : ViewModelActionsTest
	{
		Establish context = () => session = sut.CreateSession( "SP" );

		It should_execute_part_1_in_correct_order = () => sut.P1.ShouldEqual( 1 );
		It should_execute_part_2_in_correct_order = () => sut.P2.ShouldEqual( 3 );
		It should_execute_part_3_in_correct_order = () => sut.P3.ShouldEqual( 2 );
		It should_execute_part_4_in_correct_order = () => sut.P4.ShouldEqual( 4 );
		It should_execute_part_5_in_correct_order = () => sut.P5.ShouldEqual( 5 );
	}

	public class when_sequential_group_contains_nested_sequential_group : ViewModelActionsTest
	{
		Establish context = () => session = sut.CreateSession( "SS" );

		It should_execute_part_1_in_correct_order = () => sut.P1.ShouldEqual( 1 );
		It should_execute_part_2_in_correct_order = () => sut.P2.ShouldEqual( 2 );
		It should_execute_part_3_in_correct_order = () => sut.P3.ShouldEqual( 3 );
		It should_execute_part_4_in_correct_order = () => sut.P4.ShouldEqual( 4 );
		It should_execute_part_5_in_correct_order = () => sut.P5.ShouldEqual( 5 );
	}

	public class when_group_contains_initializers_and_finalizers : ViewModelActionsTest
	{
		Establish context = () => session = sut.CreateSession( "IF" );

		It should_execute_initializer_prior_to_parts = () => sut.P1.ShouldEqual( 2 );
		It should_execute_finalizer_at_the_end = () => sut.P2.ShouldEqual( 3 );
	}

	public class when_part_completes_synchronously_in_parallel_action : ViewModelActionsTest
	{
		Establish context = () => session = sut.CreateSession( "PSP" );

		It should_run_without_errors = () => session.State.ShouldEqual( SessionState.Complete );
	}

	public class when_part_completes_synchronously_in_sequential_action : ViewModelActionsTest
	{
		Establish context = () => session = sut.CreateSession( "PSS" );

		It should_run_without_errors = () => session.State.ShouldEqual( SessionState.Complete );
	}

	public class ActionViewModel : ViewModel
	{
		int _counter = 0;

		public ActionViewModel()
		{
			RegisterAction()
				.AddPart( Processor1 );

			RegisterAction( "ToExtend" )
				.AddPart( Processor1 );

			ExtendAction( "ToExtend" )
				.AddPart( Processor2 )
				.Sequential();

			RegisterAction( "PP" )
				.AddPart( Processor1 )
				.AddGroup()
					.AddPart( Processor2 )
					.AddPart( Processor3 )
					.AddPart( Processor4 )
					.Parallel()
				.AddPart( Processor5 )
				.Parallel();

			RegisterAction( "PS" )
				.AddPart( Processor1 )
				.AddGroup()
					.AddPart( Processor2 )
					.AddPart( Processor3 )
					.AddPart( Processor4 )
					.Sequential()
				.AddPart( Processor5 )
				.Parallel();

			RegisterAction( "SP" )
				.AddPart( Processor1 )
				.AddGroup()
					.AddPart( Processor2 )
					.AddPart( Processor3 )
					.AddPart( Processor4 )
					.Parallel()
				.AddPart( Processor5 )
				.Sequential();

			RegisterAction( "SS" )
				.AddPart( Processor1 )
				.AddGroup()
					.AddPart( Processor2 )
					.AddPart( Processor3 )
					.AddPart( Processor4 )
					.Sequential()
				.AddPart( Processor5 )
				.Sequential();

			RegisterAction( "IF" )
				.AddInitializer( s => GetCounter() )
				.AddPart( Processor1 )
				.AddFinalizer( s => P2 = GetCounter() );

			RegisterAction( "PSP" )
				.AddPart( session => null );

			RegisterAction( "PSS" )
				.AddPart( session => null )
				.Sequential();
		}

		int GetCounter()
		{
			return Interlocked.Increment( ref _counter );
		}

		public int P1 { get; private set; }
		public int P2 { get; private set; }
		public int P3 { get; private set; }
		public int P4 { get; private set; }
		public int P5 { get; private set; }

		public async Task Processor1( Session session )
		{
			await Task.Delay( 500 );

			P1 = GetCounter();
		}

		public async Task Processor2( Session session )
		{
			await Task.Delay( 300 );

			P2 = GetCounter();
		}

		public async Task Processor3( Session session )
		{
			await Task.Delay( 100 );

			P3 = GetCounter();
		}

		public async Task Processor4( Session session )
		{
			await Task.Delay( 700 );

			P4 = GetCounter();
		}

		public async Task Processor5( Session session )
		{
			await Task.Delay( 200 );

			P5 = GetCounter();
		}
	}
}
