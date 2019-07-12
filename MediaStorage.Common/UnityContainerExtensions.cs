using System;
using System.Reflection;
using Unity;
using Unity.Lifetime;

namespace MediaStorage.Common
{
    public static class UnityContainerExtensions
    {
        public static void RegisterAllRepositories(this IUnityContainer container, Type openGenericType, Assembly targetAssembly, Type lifeTimeManager)
        {
            if (!openGenericType.IsGenericTypeDefinition)
            {
                throw new ArgumentException("typeToRegister must be an open generic type", "typeToRegister");
            }

            foreach (Type type in targetAssembly.GetExportedTypes())
            {
                if (openGenericType.IsInterface)
                {
                    RegisterInterfaceTypes(container, openGenericType, type, type, lifeTimeManager);
                }
                else
                {
                    RegisterBaseTypes(container, openGenericType, type, type, lifeTimeManager);
                }
            }
        }
        private static void RegisterInterfaceTypes(IUnityContainer container, Type openGenericType, Type targetType, Type typeToRegister, Type lifeTimeManager)
        {
            foreach (Type interfaceType in targetType.GetInterfaces())
            {
                if ((interfaceType.IsGenericType
                    && !interfaceType.ContainsGenericParameters
                    && openGenericType.IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                    || interfaceType.IsAssignableFrom(typeToRegister))
                {
                    container.RegisterType(interfaceType, typeToRegister, (ITypeLifetimeManager)Activator.CreateInstance(lifeTimeManager));
                }
            }
        }

        private static void RegisterBaseTypes(IUnityContainer container, Type openGenericType, Type targetType, Type typeToRegister, Type lifeTimeManager)
        {
            if (targetType.BaseType != null && targetType.BaseType != typeof(object))
            {
                if (targetType.BaseType.IsGenericType && openGenericType.IsAssignableFrom(targetType.BaseType.GetGenericTypeDefinition()))
                {
                    container.RegisterType(targetType.BaseType, typeToRegister, (ITypeLifetimeManager)Activator.CreateInstance(lifeTimeManager));
                }
                else
                {
                    RegisterBaseTypes(container, openGenericType, targetType.BaseType, typeToRegister, lifeTimeManager);
                }
            }
        }
    }
}
