// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace fxpa
{
    public static class ReflectionHelper
    {
        
        /// <summary>
        /// Will return all the properties and methods of a given type, 
        /// that have the designated return type and take no parameter. Used in automated statistics.
        /// </summary>
        /// <param name="individualType"></param>
        /// <returns></returns>
        static public MethodInfo[] GetTypePropertiesAndMethodsByReturnType(Type objectType, Type[] returnTypes)
        {
            List<MethodInfo> resultList = new List<MethodInfo>();

            MethodInfo[] allMethods = objectType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (MethodInfo methodInfo in allMethods)
            {
                bool doesMatchReturnType = false;
                foreach (Type type in returnTypes)
                {
                    if (methodInfo.ReturnType == type)
                    {
                        doesMatchReturnType = true;
                        break;
                    }
                }

                if (methodInfo.GetParameters().Length == 0 && doesMatchReturnType)
                {// No params, proper return type - this is the one.
                    resultList.Add(methodInfo);
                }
            }

            MethodInfo[] resultArray = new MethodInfo[resultList.Count];
            resultList.CopyTo(resultArray);
            return resultArray;
        }

        /// <summary>
        /// Will create for you the needed instances of the given type children types
        /// with DEFAULT CONSTRUCTORS only, no params.
        /// </summary>
        static public List<TypeRequired> GetTypeChildrenInstances<TypeRequired>(System.Reflection.Assembly assembly)
        {
            List<TypeRequired> resultingInstances = new List<TypeRequired>();

            // Collect all the types that match the description
            List<Type> blockTypes = ReflectionHelper.GetTypeChildrenTypes(typeof(TypeRequired), assembly);

            foreach (Type blockType in blockTypes)
            {
                System.Reflection.ConstructorInfo[] constructorInfo = blockType.GetConstructors();

                if (constructorInfo == null || constructorInfo.Length == 0 ||
                    blockType.IsAbstract || blockType.IsClass == false)
                {
                    continue;
                }

                resultingInstances.Add((TypeRequired)constructorInfo[0].Invoke(null));
            }

            return resultingInstances;
        }

        //static public int GetEnumValueIndex(System.Type enumType, string valueName)
        //{
        //    string[] names = System.Enum.GetNames(enumType);
        //    for (int i = 0; i < names.Length; i++)
        //    {
        //        if (names[i] == valueName)
        //        {
        //            return i;
        //        }
        //    }
        //    throw new Exception("Invalid enum value name passed in.");
        //}

        /// <summary>
        /// Loads all the assemblies in the directory of the executing (entry) assembly and searches 
        /// them for inheritors of the given type. SLOW!
        /// </summary>
        /// <returns></returns>
        //static public List<Type> GetCollectTypeChildrenTypesFromRelatedAssemblies(Type typeSearched)
        //{
        //    List<Type> result = new List<Type>();

        //    // Load all the assemblies in the directory of the current application and try to find
        //    // inheritors of AIndividual in them, then gather those in the list.
        //    string path = Assembly.GetEntryAssembly().Location;
        //    path = path.Remove(path.LastIndexOf('\\'));
        //    string[] dllFiles = System.IO.Directory.GetFiles(path, "*.dll");

        //    foreach (string file in dllFiles)
        //    {
        //        Assembly assembly;
        //        try
        //        {
        //            assembly = Assembly.LoadFile(file);
        //        }
        //        catch (Exception)
        //        {// This DLL was not a proper assembly, disregard.
        //            continue;
        //        }
        //        // Try to find typeSearched inheritors in this assembly.
        //        result.AddRange(ReflectionSupport.GetTypeChildrenTypes(typeSearched, assembly));
        //    }
        //    return result;
        //}

        /// <summary>
        /// 
        /// </summary>
        static public Assembly[] GetApplicationEntryAssemblyReferencedAssemblies()
        {
            return GetReferencedAssemblies(Assembly.GetEntryAssembly());
        }

        /// <summary>
        /// 
        /// </summary>
        static public Assembly[] GetApplicationEntryAssemblyAndReferencedAssemblies()
        {
            return GetReferencedAndInitialAssembly(Assembly.GetEntryAssembly());
        }

        /// <summary>
        /// 
        /// </summary>
        static public Assembly[] GetReferencedAndInitialAssembly(Assembly initialAssembly)
        {
            AssemblyName[] names = initialAssembly.GetReferencedAssemblies();
            Assembly[] assemblies = new Assembly[names.Length + 1];
            for (int i = 0; i < names.Length; i++)
            {
                assemblies[i] = Assembly.Load(names[i]);
            }
            assemblies[assemblies.Length - 1] = initialAssembly;
            return assemblies;
        }

        /// <summary>
        /// 
        /// </summary>
        static public Assembly[] GetReferencedAssemblies(Assembly initialAssembly)
        {
            AssemblyName[] names = initialAssembly.GetReferencedAssemblies();
            Assembly[] assemblies = new Assembly[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                assemblies[i] = Assembly.Load(names[i]);
            }
            return assemblies;
        }

        static public List<Type> GatherTypeChildrenTypesFromAssemblies(Type parentType, Assembly[] assemblies)
        {
            return GatherTypeChildrenTypesFromAssemblies(parentType, assemblies, false, true);
        }

        /// <summary>
        /// This will look for children types in Entry, Current, Executing, Calling assemly, 
        /// as well as assemblies with names specified and found in the directory of the current application.
        /// </summary>
        /// <returns></returns>
        static public List<Type> GatherTypeChildrenTypesFromAssemblies(Type parentType, Assembly[] assemblies, bool allowOnlyClasses, bool allowAbstracts)
        {
            List<Type> resultingTypes = new List<Type>();
            if (assemblies == null)
            {
                return resultingTypes;
            }

            foreach (Assembly assembly in assemblies)
            {
                List<Type> types = ReflectionHelper.GetTypeChildrenTypes(parentType, assembly);
                foreach (Type type in types)
                {
                    if ((allowOnlyClasses == false || type.IsClass)
                        && (allowAbstracts || type.IsAbstract == false))
                    {
                        resultingTypes.Add(type);
                    }
                }
            }

            return resultingTypes;
        }

        static public List<Type> GatherTypeChildrenTypesFromAssembliesWithMatchingConstructor(Type parentType, bool exactParameterTypeMatch, Assembly[] assemblies, Type[] constructorParametersTypes)
        {
            List<Type> candidateTypes = GatherTypeChildrenTypesFromAssemblies(parentType, assemblies);
            List<Type> resultingTypes = new List<Type>();
            foreach (Type type in candidateTypes)
            {
                ConstructorInfo constructorInfo = type.GetConstructor(constructorParametersTypes);
                if (constructorInfo != null)
                {
                    bool isValid = true;
                    if (exactParameterTypeMatch)
                    {// Perform check for exact type match, evade parent classes.

                        ParameterInfo[] parameterInfos = constructorInfo.GetParameters();
                        for (int i = 0; i < parameterInfos.Length; i++)
                        {
                            if (parameterInfos[i].ParameterType != constructorParametersTypes[i])
                            {// Not good, skip.
                                isValid = false;
                                break;
                            }
                        }
                    }

                    if (isValid)
                    {
                        resultingTypes.Add(type);
                    }
                }
            }

            return resultingTypes;
        }


        /// <summary>
        /// Collect them from a given assembly.
        /// </summary>
        static public List<Type> GetTypeChildrenTypes(Type typeSearched, System.Reflection.Assembly assembly)
        {
            Type[] types = assembly.GetTypes();

            List<Type> result = new List<Type>();
            foreach (Type type in types)
            {
                if (typeSearched.IsInterface)
                {
                    List<Type> interfaces = new List<Type>(type.GetInterfaces());
                    if (interfaces.Contains(typeSearched))
                    {
                        result.Add(type);
                    }
                }
                else
                if (type.IsSubclassOf(typeSearched))
                {
                    result.Add(type);
                }
            }
            return result;
        }
    }
}

