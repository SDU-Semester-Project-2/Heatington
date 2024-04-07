// See https://aka.ms/new-console-template for more information

using Heatington.AssetManager;

Console.WriteLine("Hello, World!");

AssetManager AM = new AssetManager();

await AM.LoadAssets();
