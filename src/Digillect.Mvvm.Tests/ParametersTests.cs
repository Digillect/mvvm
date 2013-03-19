using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Machine.Specifications;
using Moq;

using It = Machine.Specifications.It;
using MoqIt = Moq.It;

namespace Digillect.Mvvm.Tests
{
	public class when_creating_Parameters_using_static_method_From
	{
		static Parameters parameters;

		Because of = () => parameters = Parameters.Create( "hello", "world" );

		It should_return_valid_object = () => parameters.ShouldNotBeNull();
		It should_have_only_one_parameter = () => parameters.Count().ShouldEqual( 1 );
	}

	public class when_adding_parameter_to_Parameters
	{
		static Parameters parameters;
		static Parameters result;

		Establish context = () =>
		{
			parameters = new Parameters();
		};

		Because of = () => result = parameters.Add( "year", 2012 );

		It should_return_valid_object = () => result.ShouldNotBeNull();
		It should_return_same_instance_of_Parameters = () => result.ShouldBeTheSameAs( parameters );
		It should_have_two_parameters = () => result.Count().ShouldEqual( 1 );
	}

	public class when_chaining_Parameters_calls
	{
		static Parameters parameters;

		Because of = () => parameters = Parameters.Create( "hello", "world" ).Add( "year", 2012 );

		It should_return_valid_object = () => parameters.ShouldNotBeNull();
		It should_have_two_parameters = () => parameters.Count().ShouldEqual( 2 );
	}

	public class when_creating_Parameters_using_static_method_From_and_passing_null_as_the_name
	{
		static Parameters parameters;
		static Exception exception;

		Because of = () => exception = Catch.Exception( () => parameters = Parameters.Create( null, "world" ) );

		It should_throw_ArgumentNullException = () => exception.ShouldBeOfType<ArgumentNullException>();
		It should_not_return_valid_object = () => parameters.ShouldBeNull();
	}

	public class when_creating_Parameters_using_static_method_From_and_passing_null_as_the_value
	{
		static Parameters parameters;
		static Exception exception;

		Because of = () => exception = Catch.Exception( () => parameters = Parameters.Create( "hello", (string) null ) );

		It should_throw_ArgumentNullException = () => exception.ShouldBeOfType<ArgumentNullException>();
		It should_not_return_valid_object = () => parameters.ShouldBeNull();
	}

	public class when_adding_parameter_to_Parameters_and_passing_null_as_the_name
	{
		static Parameters parameters;
		static Parameters result;
		static Exception exception;

		Establish context = () =>
		{
			parameters = new Parameters();
		};

		Because of = () => exception = Catch.Exception( () => result = parameters.Add( null, "world" ) );

		It should_throw_ArgumentNullExceptions = () => exception.ShouldBeOfType<ArgumentNullException>();
		It should_not_return_valid_object = () => result.ShouldBeNull();
	}

	public class when_adding_parameter_to_Parameters_and_passing_null_as_the_value
	{
		static Parameters parameters;
		static Parameters result;
		static Exception exception;

		Establish context = () =>
		{
			parameters = new Parameters();
		};

		Because of = () => exception = Catch.Exception( () => result = parameters.Add( "hello", (string) null ) );

		It should_throw_ArgumentNullExceptions = () => exception.ShouldBeOfType<ArgumentNullException>();
		It should_not_return_valid_object = () => result.ShouldBeNull();
	}

	public class when_retrieveing_value_from_Parameters_and_parameter_exists
	{
		static Parameters parameters;
		static string result;

		Establish context = () =>
		{
			parameters = Parameters.Create( "hello", "world" );
		};

		Because of = () => result = parameters.Get<string>( "hello" );

		It should_return_valid_value = () => result.ShouldEqual( "world" );
	}

	public class when_retrieveing_value_from_Parameters_without_specifying_default_value_and_parameter_does_not_exists
	{
		static Parameters parameters;
		static int result;

		Establish context = () =>
		{
			parameters = new Parameters();
		};

		Because of = () => result = parameters.Get<int>( "hello" );

		It should_return_default_type_value = () => result.ShouldEqual( 0 );
	}

	public class when_retrieveing_value_from_Parameters_with_specifying_default_value_and_parameter_does_not_exists
	{
		static Parameters parameters;
		static int result;

		Establish context = () =>
		{
			parameters = new Parameters();
		};

		Because of = () => result = parameters.Get<int>( "hello", 42 );

		It should_return_default_type_value = () => result.ShouldEqual( 42 );
	}

	public class when_enumerating_Parameters
	{
		static Parameters parameters;
		static IEnumerator<KeyValuePair<string, object>> enumerator;

		Establish context = () =>
		{
			parameters = Parameters.Create( "hello", "world" ).Add( "year", 2012 );
		};

		Because of = () => enumerator = parameters.GetEnumerator();

		It should_return_valid_enumerator = () => enumerator.ShouldNotBeNull();
	}
}
