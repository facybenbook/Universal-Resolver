﻿///**
// * Author:    Vinh Vu Thanh
// * This class is a part of Unity IoC project that can be downloaded free at 
// * https://github.com/game-libgdx-unity/UnityEngine.IoC
// * (c) Copyright by MrThanhVinh168@gmail.com
// **/
using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityIoC;

public class ContextPlayMode
{
    private Context _context;

    [SetUp]
    public void Setup()
    {
        _context = new Context();
        Debug.Log("Setup test for " + GetType());
    }
    
    [UnityTest]
    public IEnumerator t03_Create_Monobehavior_Instance()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;
        _context._implementClass = Resources.Load<ImplementClass>("TestConfiguration");


        //wait for start() called
        yield return null;
    }

    [Test]
    public void t04_NoBind_CreateMonobehavior()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;
//        _context.implementClasses = Resources.Load<ImplementClass>("TestConfiguration");

    }

    [UnityTest]
    public IEnumerator t05_ThrowException_NoBind_CreateFromInterface()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;
        _context._implementClass = Resources.Load<ImplementClass>("TestConfiguration");


        //wait for start() called
        yield return null;

        Assert.Throws<InvalidOperationException>(() =>
        {
            //act: will throw exceptions when u try to resolve an unregistered interface 
            var obj = _context.Resolve<IContainer>();
            //assert
            Assert.IsNotNull(obj);
        });
    }

    [UnityTest]
    public IEnumerator t06_BindInstance_CreateFromInterface()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;
        _context._implementClass = Resources.Load<ImplementClass>("TestConfiguration");

        _context.Bind<IList>(new System.Collections.Generic.List<object>());


        //wait for start() called
        yield return null;

        var obj = _context.Resolve<IList>();

        //assert
        Assert.IsNotNull(obj);
    }

    [UnityTest]
    public IEnumerator t07_BindClass_CreateSingleton()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;

        _context.Bind<IList, List<object>>(LifeCycle.Singleton);

        var list = _context.Resolve<IList>();
        Assert.IsNotNull(list);

        list.Add(new object());


        //wait for start() called
        yield return null;

        var obj = _context.Resolve<IList>();

        //assert
        Assert.IsNotNull(obj);
        //assert
        Assert.AreEqual(obj.Count, 1);
    }

    [UnityTest]
    public IEnumerator t08_BindClass_CreateSingleton_ResolveTransient()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;

        _context.Bind<IList, List<object>>(LifeCycle.Singleton);

        var list = _context.Resolve<IList>();
        Assert.IsNotNull(list);

        list.Add(new object());


        //wait for start() called
        yield return null;

        var singleton = _context.Resolve<IList>();

        //assert
        Assert.IsNotNull(singleton);
        //assert
        Assert.AreEqual(singleton.Count, 1);
    }

    [UnityTest]
    public IEnumerator t08a_BindClass_CreateSingleton_ResolveTransient()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;

        _context.Bind<IList, List<object>>(LifeCycle.Singleton);

        var list = _context.Resolve<IList>();
        Assert.IsNotNull(list);

        list.Add(new object());


        //wait for start() called
        yield return null;

        var transient = _context.Resolve<IList>(LifeCycle.Transient);
        //assert
        Assert.IsNotNull(transient);
        //assert
        Assert.IsTrue(transient.Count == 0);
    }

    [UnityTest]
    public IEnumerator t08b_BindClass_CreateSingleton_ResolveTransient()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;

        _context.Bind<IList, List<object>>(LifeCycle.Singleton);

        var list = _context.Resolve<IList>();
        Assert.IsNotNull(list);

        list.Add(new object());


        //wait for start() called
        yield return null;


        var transient = _context.Resolve<IList>(LifeCycle.Transient);
        //assert
        Assert.IsNotNull(transient);
        //assert
        Assert.AreEqual(transient.Count, 0);


        var singleton2 = _context.Resolve<IList>();
        //assert
        Assert.IsNotNull(singleton2);
        //assert
        Assert.AreEqual(singleton2.Count, 1);
    }

    [UnityTest]
    public IEnumerator t09_ThrowException_NoBind_CreateFromInterface()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;
        _context._implementClass = Resources.Load<ImplementClass>("TestConfiguration");


        //wait for start() called
        yield return null;

        //assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            //act: will throw exceptions if u try to resolve an unregistered interface 
            var obj = _context.Resolve<IList>();
            //assert
            Assert.IsNotNull(obj);
        });
    }

    [UnityTest]
    public IEnumerator t10_Binding_Inject_NotNull()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var testObj = _context.Resolve<TestClass>();
        Assert.IsNotNull(testObj);
        Assert.IsNotNull(testObj.SomeInterface);
    }

    [UnityTest]
    public IEnumerator t11_Binding_Inject_Transient()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var testObj = _context.Resolve<TestClass>(LifeCycle.Singleton);
        testObj.SomeInterface.SomeIntProperty = 10;

        var anotherTestObj = _context.Resolve<TestClass>(LifeCycle.Transient);
        Assert.AreEqual(anotherTestObj.SomeInterface.SomeIntProperty, 0);
    }

    [UnityTest]
    public IEnumerator t12_Binding_Inject_Transient_Singleton()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var testObj = _context.Resolve<TestClass>(LifeCycle.Singleton);
        testObj.SomeInterface.SomeIntProperty = 10;

        var anotherTestObj = _context.Resolve<TestClass>(LifeCycle.Transient);
        anotherTestObj.SomeInterface.SomeIntProperty = 5;

        var stillTestObj = _context.Resolve<TestClass>(LifeCycle.Singleton);
        Assert.IsTrue(stillTestObj.SomeInterface.SomeIntProperty == 10);
    }

    [UnityTest]
    public IEnumerator t13_Binding_Inject_Transient_Default()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var testObj = _context.Resolve<SingletonClass>();
        testObj.PropertyAsInterface.SomeIntProperty = 10;

        var anotherTestObj = _context.Resolve<SingletonClass>(LifeCycle.Transient);
        anotherTestObj.PropertyAsInterface.SomeIntProperty = 5;

        var stillTestObj = _context.Resolve<SingletonClass>();
        Assert.AreEqual(10, stillTestObj.PropertyAsInterface.SomeIntProperty);
    }

    [UnityTest]
    public IEnumerator t14_Component_Binding_Inject_NotNull()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var testObj = _context.Resolve<TestComponent>();
        Assert.IsNotNull(testObj);
        Assert.IsNotNull(testObj.PropertyAsInterface);
    }

    [UnityTest]
    public IEnumerator t15_Component_Binding_Inject_Transient()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var testObj = _context.Resolve<TestComponent>(LifeCycle.Singleton);
        testObj.PropertyAsInterface.SomeIntProperty = 10;

        var anotherTestObj = _context.Resolve<TestComponent>(LifeCycle.Transient);
        Assert.AreEqual(anotherTestObj.PropertyAsInterface.SomeIntProperty, 0);
    }

    [UnityTest]
    public IEnumerator t16_Component_Binding_Inject_Transient_Singleton()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var testObj = _context.Resolve<TestComponent>(LifeCycle.Singleton);
        testObj.PropertyAsInterface.SomeIntProperty = 10;

        var anotherTestObj = _context.Resolve<TestComponent>(LifeCycle.Transient);
        anotherTestObj.PropertyAsInterface.SomeIntProperty = 5;

        var stillTestObj = _context.Resolve<TestComponent>(LifeCycle.Singleton);
        Assert.IsTrue(stillTestObj.PropertyAsInterface.SomeIntProperty == 10);
    }

    [UnityTest]
    public IEnumerator t17_Component_Binding_Inject_Transient_Default()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var testObj = _context.Resolve<SingletonComponent>();
        testObj.PropertyAsInterface.SomeIntProperty = 10;

        var anotherTestObj = _context.Resolve<SingletonComponent>(LifeCycle.Transient);
        anotherTestObj.PropertyAsInterface.SomeIntProperty = 5;

        var stillTestObj = _context.Resolve<SingletonComponent>();
        Assert.AreEqual(10, stillTestObj.PropertyAsInterface.SomeIntProperty);
    }

    [UnityTest]
    public IEnumerator t18_Transient_Component_Binding_Inject_Transient_Default()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var testObj = _context.Resolve<TestComponent>();
        testObj.PropertyAsInterface.SomeIntProperty = 10;

        var singletonTestObj = _context.Resolve<TestComponent>(LifeCycle.Singleton);
        singletonTestObj.PropertyAsInterface.SomeIntProperty = 5;

        var anotherTransTestObj = _context.Resolve<TestComponent>(LifeCycle.Transient);
        anotherTransTestObj.PropertyAsInterface.SomeIntProperty = 10;

        var anotherTransTestObj2 = _context.Resolve<TestComponent>(LifeCycle.Transient);
        Assert.AreEqual(anotherTransTestObj2.PropertyAsInterface.SomeIntProperty, 0);

        var stillSingletonObj = _context.Resolve<TestComponent>(LifeCycle.Singleton);
        Assert.AreEqual(stillSingletonObj.PropertyAsInterface.SomeIntProperty, 5);
    }

    [UnityTest]
    public IEnumerator t19_Simple_Transient_Class_Binding_Inject_Singleton_Property()
    {
        //arrange: do setup context obj for this test

        _context.enableAutomaticBinding = false;


        yield return null;

        var obj = _context.Resolve<SingletonClass>();
        obj.PropertyAsInterface.SomeIntProperty = 2;

        var testObj = _context.Resolve<SingletonClass>(LifeCycle.Transient);
        testObj.PropertyAsInterface.SomeIntProperty = 10;

        var singletonObj = _context.Resolve<SingletonClass>();

        singletonObj = _context.Resolve<SingletonClass>();
    }

   
}