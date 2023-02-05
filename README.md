## Mocking with Moq 4 and xUnit
- by Jason Roberts

- OVERVIEW:
    - Writing unit tests is hard when dependencies between classes make it tough to separate what's being tested from the rest of the system. 
    - Moq, the most popular mocking library for .NET, makes it easy to create mock dependencies to make testing easier.
    - Methods. Properties. Throwing events and exceptions.

- GETTING STARTED WITH MOCKING & MOQ:
    - Overview:
        - Components/classes. Test a portion of the system in isolation. Component has a dependency on another part of the system. 
        - Mocking: Replacing the actual dependency that would be used in production with a test-onlt version.
            - To do so allows for us to enable easier isolation of the SUT.
    - Why use Mock objects?
        - Improve test execution speed. Slow algorithms. External resources. database. We service.
        - Support parallel development streams. (Real object not yet developed.)
        - Improve test reliability. e.g.: Unreliable Web server.
        - Reduce development/testing costs. e.g.: Mainframe interfacing.
        - Test when non-deterministic dependency. e.g.: Current date/time code replaced with a guarentee.
    - What's a unit in unit testing?
        - Low level details of the class.
        - Highly focused on a specific part.
        - Quick to execute.
        - Easier to test all logical paths. A "unit" can be a series of classes.
        - "A unit is a situational thing."
            - "The team decides what makes sense to be a unit for the purpose of understanding the system and the associated system testing."
        - Units of behavior over units of implementation.
    - Fakes, stubs, mocks, and test doubles.
        - "Test double:" An umbrella term. A generic term for any case where you replace a production object for testing purposes.
            - Fakes: Working implementation. Not suitable for production. e.g.: EF COre in-memory provider.
            - Dummies: Passwed around. Never used/accessed. Satidsfied parameters. *
            - Stubs: Provide answers to calls. Method return values. *
            - Mocks: Expect/verify calls. Properties. Methods. *
                - * Moq.
    - Demo code overview:
    - Add a new unit test project.
    - Write initial tests.
    - Add a new dependency.
    - Cannot supply null.
    - SUMMARY: Isolation using test-time -only dependencies.

- CONFIGURING MOCKED METHODS:
    - Install Moq.
    - Argument matching: (Predicate: Function that returns a boolean result.)
        ```csharp
            m.Setup(m => m.IsValid("x")).Returns(true);
            m.Setup(m => m.IsValid(It.IsAny<string>())).Returns(true);
            m.Setup(m => m.IsValid(It.Is<string>(number => number.StartsWith("x")))).Returns(true);
            m.Setup(m => m.IsValid(It.IsInRange<string>("x", "z", Moq.Range.Inclusive))).Returns(true);
            m.Setup(m => m.IsValid(It.IsIn("x", "y", "z"))).Returns(true);
            m.Setup(m => m.IsValid(It.IsRegex("[x-z]"))).Returns(true);
        ```
    - Strict and loose mocks. By default, a loose Mock will be created.
        - MockBehavior.Strict. Throw an exception if a mocked method is called but has not been set up.
        - MockBehavior.Loose. Never throw exceptions, even if a mocked method is called but has not been set up.
        - MockBehavior.Loose. Return default values for value types, null for reference types, ampty array/enumerable.
        - MockBehavior.Default:
    - Mock methods with 'out' parameters.
    - Argument matching: (Predicate: Function that returns a boolean result.)
        ```csharp
            var person1 = new Person();
            var person2 = new Person();
            var gateway = new Mock<IGateway>();
            gateway.Setup(g => g.Execute(ref person1)).Returns(-1);

            var sut = new Processor(gateway.Object);
            Assert.Equal(-1, sut.Process(person1);
            Assert.Equal(0, sut.Process(person2);
        ```
        ```csharp
            var gateway = new Mock<IGateway>();
            gateway.Setup(g => g.Execute(ref It.Ref<Person>.IsAny)).Returns(-1);
            Assert.Equal(-1, sut.Process(person1);
            Assert.Equal(-1, sut.Process(person2);
        ```
    - SUMMARY:

- CONFIGURING MOCK OBJECT PROPERTIES:
    - Configure a mocked property. Either a literal value or a function return value:
        ```csharp
            validator.Setup(v => v.LicenseKey).Returns("EXPIRED");

            // Note: Execution deferred.
            validator.Setup(v => v.LicenseKey).Returns(GetLicenseKey);
        ```
    - Auto-mocking of property hierarchies:
        - Whan a mock can actually be created from a reference type: Interfaces. Abstract class. Non-sealed class.
            ```csharp
                MockObject.DefaultValue = DefaultValue.Mock;
            ```
    - By default, Mock properties do not recall changes made to them. NOTE: Use SetupAllProperties() before specific property setup.
        ```csharp
            validator.SetupProperty(v => v.ValidationMode);
            validator.SetupAllProperties();
        ```
    - SUMMARY:

- IMPLEMENTATING BEHAVIOR VERFICATION TESTS:
    - Behavior testing versus state-based testing.
        - State-based testing: SUT interacts with the mock object. 
            - Read the result of the method call and make an assert against it.
            - Asserts check the SUT.
        - Behavior: Check the mock object. Was a method called? Was a property accessed?
        - e.g.: Product cache. Invalidated. From DB or from cache? 
        - Mock DB. Cache asks for information via Mock DB. And we write tests validating the Db call.
    - Verify a method was called. With custom error message.
        ```csharp
            // Behavior testing. Expected invocation on the mock.
            validator.Verify(v => v.IsValid(SPECIFIC_VALUE), $"Invalid FrequentFlyerNumber: {SPECIFIC_VALUE}");

            validator.Verify(v => v.IsValid(It.IsAny<string>()), Times.Never);
            validator.VerifyGet(v => v.LicenseKey, Times.Never);
            validator.VerifySet(v => v.ValidationMode = ValidationMode.Detailed);
            validator.VerifyNoOtherCalls();
        ```
    - SUMMARY:

- USING ADDITIONAL MOQ MOCKING TECHNIQUES:
    - Throw exceptions from Mock objects:
    - Raising Events:
        - Manually:
        - Automatically:
    - Return different results for sequential calls:
    - Mock virtual (protected) members of concrete types:
    - Improve mock setup readability with LINQ:
    - Refactor:
    - Generic Type argument matching:
    - Mocking async method return values: