#region Copyright (c) 2011-2013 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman)
// Copyright (c) 2011-2013 Gregory Nickonov and Andrew Nefedkin (Actis® Wunderman).
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Machine.Specifications;
using Moq;

using It = Machine.Specifications.It;
using MoqIt = Moq.It;

using Digillect.Mvvm;
using Digillect.Mvvm.Services;

namespace Digillect.Mvvm.Tests
{
	public class ViewModelErrorHandlingSpec
	{
		public static ErrorRaisingViewModel viewModel;
		public static Mock<IViewModelExceptionHandlingService> ehs;
		public static Exception ehsException;
		public static Exception eventException;
		public static Exception resultException;

		Establish context = () =>
		{
			viewModel = new ErrorRaisingViewModel();
			ehs = new Mock<IViewModelExceptionHandlingService>();

			viewModel.ViewModelExceptionHandlingService = ehs.Object;

			ehsException = null;
			eventException = null;
			resultException = null;
		};
	}

	public class when_ViewModel_raises_exception_in_synchronous_loading_part_and_it_is_handled_by_IViewModelExceptionHandlingService : ViewModelErrorHandlingSpec
	{
		Establish context = () =>
		{
			ehs.Setup( x => x.HandleException( viewModel, MoqIt.IsAny<Session>(), MoqIt.IsAny<Exception>() ) )
				.Callback( ( ViewModel vm, Session session, Exception ex ) => { ehsException = ex; } )
				.Returns( true )
				.Verifiable();

			viewModel.SessionAborted += ( sender, ea ) => eventException = ea.Exception;
		};

		Because of = () => resultException = Catch.Exception( () => viewModel.LoadAndThrowSync().Await() );

		It should_call_IViewModelExceptionHandlingService = () => ehs.Verify();
		It should_pass_an_exception_to_SessionAborted_event = () => eventException.ShouldNotBeNull();
		It should_pass_an_exception_to_error_handling_service = () => ehsException.ShouldNotBeNull();
		It should_not_rethrow_exception_to_caller = () => resultException.ShouldBeNull();
	}

	public class when_ViewModel_raises_exception_in_synchronous_loading_part_and_it_is_not_handled_by_IViewModelExceptionHandlingService : ViewModelErrorHandlingSpec
	{
		Establish context = () =>
		{
			ehs.Setup( x => x.HandleException( viewModel, MoqIt.IsAny<Session>(), MoqIt.IsAny<Exception>() ) )
				.Callback( ( ViewModel vm, Session session, Exception ex ) => { ehsException = ex; } )
				.Returns( false )
				.Verifiable();

			viewModel.SessionAborted += ( sender, ea ) => eventException = ea.Exception;
		};

		Because of = () => resultException = Catch.Exception( () => viewModel.LoadAndThrowSync().Await() );

		It should_call_IViewModelExceptionHandlingService = () => ehs.Verify();
		It should_pass_an_exception_to_SessionAborted_event = () => eventException.ShouldNotBeNull();
		It should_pass_an_exception_to_error_handling_service = () => ehsException.ShouldNotBeNull();
		It should_rethrow_exception_to_caller = () => resultException.ShouldNotBeNull();
	}

	public class when_ViewModel_raises_exception_in_asynchronous_loading_part_and_it_is_handled_by_IViewModelExceptionHandlingService : ViewModelErrorHandlingSpec
	{
		Establish context = () =>
		{
			ehs.Setup( x => x.HandleException( viewModel, MoqIt.IsAny<Session>(), MoqIt.IsAny<Exception>() ) )
				.Callback( ( ViewModel vm, Session session, Exception ex ) => { ehsException = ex; } )
				.Returns( true )
				.Verifiable();

			viewModel.SessionAborted += ( sender, ea ) => eventException = ea.Exception;
		};

		Because of = () => resultException = Catch.Exception( () => viewModel.LoadAndThrowAsync().Await() );

		It should_call_IViewModelExceptionHandlingService = () => ehs.Verify();
		It should_pass_an_exception_to_SessionAborted_event = () => eventException.ShouldNotBeNull();
		It should_pass_an_exception_to_error_handling_service = () => ehsException.ShouldNotBeNull();
		It should_not_rethrow_exception_to_caller = () => resultException.ShouldBeNull();
	}

	public class when_ViewModel_raises_exception_in_asynchronous_loading_part_and_it_is_not_handled_by_IViewModelExceptionHandlingService : ViewModelErrorHandlingSpec
	{
		Establish context = () =>
		{
			ehs.Setup( x => x.HandleException( viewModel, MoqIt.IsAny<Session>(), MoqIt.IsAny<Exception>() ) )
				.Callback( ( ViewModel vm, Session session, Exception ex ) => { ehsException = ex; } )
				.Returns( false )
				.Verifiable();

			viewModel.SessionAborted += ( sender, ea ) => eventException = ea.Exception;
		};

		Because of = () => resultException = Catch.Exception( () => viewModel.LoadAndThrowAsync().Await() );

		It should_call_IViewModelExceptionHandlingService = () => ehs.Verify();
		It should_pass_an_exception_to_SessionAborted_event = () => eventException.ShouldNotBeNull();
		It should_pass_an_exception_to_error_handling_service = () => ehsException.ShouldNotBeNull();
		It should_rethrow_exception_to_caller = () => resultException.ShouldNotBeNull();
	}

	public class ErrorRaisingViewModel : ViewModel
	{
		public ErrorRaisingViewModel()
		{
			RegisterPart( "main", LoadMainPart );
		}

		public Task LoadAndThrowSync()
		{
			return Load( new Session().AddParameter( "ThrowSync", true ) );
		}

		public Task LoadAndThrowAsync()
		{
			return Load( new Session().AddParameter( "ThrowAsync", true ) );
		}

		protected override void LoadSession( Session session )
		{
			base.LoadSession( session );

			if( session.Parameters.GetValue<bool>( "ThrowSync" ) )
			{
				throw new NotImplementedException();
			}
		}

		private async Task LoadMainPart( Session session )
		{
			await Task.Delay( 1000 );

			if( session.Parameters.GetValue<bool>( "ThrowAsync" ) )
			{
				throw new NotImplementedException();
			}
		}
	}
}
