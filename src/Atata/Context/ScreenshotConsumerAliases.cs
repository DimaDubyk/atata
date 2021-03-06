﻿using System;
using System.Collections.Generic;

namespace Atata
{
    public static class ScreenshotConsumerAliases
    {
        private static readonly Dictionary<string, Func<IScreenshotConsumer>> AliasFactoryMap = new Dictionary<string, Func<IScreenshotConsumer>>(StringComparer.OrdinalIgnoreCase);

        static ScreenshotConsumerAliases()
        {
            Register<FileScreenshotConsumer>(File);
        }

        public static string File => nameof(File);

        public static void Register<T>(string typeAlias)
            where T : IScreenshotConsumer, new()
        {
            Register(typeAlias, () => new T());
        }

        public static void Register(string typeAlias, Func<IScreenshotConsumer> logConsumerFactory)
        {
            typeAlias.CheckNotNullOrWhitespace(nameof(typeAlias));
            logConsumerFactory.CheckNotNull(nameof(logConsumerFactory));

            AliasFactoryMap[typeAlias.ToLower()] = logConsumerFactory;
        }

        public static IScreenshotConsumer Resolve(string typeNameOrAlias)
        {
            typeNameOrAlias.CheckNotNullOrWhitespace(nameof(typeNameOrAlias));

            Func<IScreenshotConsumer> factory;

            return AliasFactoryMap.TryGetValue(typeNameOrAlias, out factory)
                ? factory()
                : ActivatorEx.CreateInstance<IScreenshotConsumer>(typeNameOrAlias);
        }
    }
}
