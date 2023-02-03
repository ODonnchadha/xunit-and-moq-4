## Mocking with Moq 4 and xUnit
- by Jason Roberts
 - Writing unit tests is hard when dependencies between classes make it tough to separate what's being tested from the rest of the system. 
 - Moq, the most popular mocking library for .NET, makes it easy to create mock dependencies to make testing easier.

- OVERVIEW:
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
    - 

- CONFIGURING MOCK OBJECT PROPERTIES:
- IMPLEMENTATING BEHAVIOR VERFICATION TESTS:
- USING ADDITIONAL MOQ MOCKING TECHNIQUES: