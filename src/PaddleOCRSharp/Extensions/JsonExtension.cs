// Copyright (c) 2021 raoyutian Authors. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if NET35
using Newtonsoft.Json;

#else
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
#endif

namespace PaddleOCRSharp.Extensions;

internal static class JsonExtension
{
#if NETFRAMEWORK
    private static JsonSerializerSettings Settings { get; set; } = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
    };

    /// <summary>
    /// Json序列化
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string SerializeObject<T>(this T obj) => JsonConvert.SerializeObject(obj, Settings);

    /// <summary>
    /// Json反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T DeserializeObject<T>(this string json) =>
        (T)JsonConvert.DeserializeObject(json, typeof(T), Settings);
#else
    /// <summary>
    /// Json序列化
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string SerializeObject<T>(this T obj) => JsonSerializer.Serialize(obj);

    public static string SerializeObject<T>(this T obj, JsonTypeInfo<T> typeInfo) =>
        JsonSerializer.Serialize(obj, typeInfo);

    public static T? DeserializeObject<T>(this string json) => JsonSerializer.Deserialize<T>(json);
    public static T? DeserializeObject<T>(this string json, JsonTypeInfo<T> typeInfo) =>
        JsonSerializer.Deserialize(json, typeInfo);
#endif
}