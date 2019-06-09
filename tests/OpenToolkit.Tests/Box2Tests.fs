namespace OpenToolkit.Tests

open Xunit
open FsCheck
open System
open FsCheck.Xunit
open System.Runtime.InteropServices
open OpenToolkit
open OpenToolkit.Mathematics

module Box2 =
    [<Properties(Arbitrary = [| typeof<OpenTKGen> |])>]
    module Constructors =
        [<Property>]
        let ``Vector constructor sets all values accordingly`` (v1 : Vector2, v2 : Vector2) =
            let b = Box2(v1, v2)
            let vMin = Vector2(Math.Min(v1.X, v2.X), Math.Min(v1.Y, v2.Y))
            let vMax = Vector2(Math.Max(v1.X, v2.X), Math.Max(v1.Y, v2.Y))

            Assert.Equal(vMin, b.Min)
            Assert.Equal(vMax, b.Max)

        [<Property>]
        let ``Float constructor should be the same as creating vectors and using the vector constructor`` (f1 : float32, f2 : float32, f3 : float32, f4 : float32) =
            let b1 = Box2(f1, f2, f3, f4)
            let b2 = Box2(Vector2(f1, f2), Vector2(f3, f4))

            Assert.Equal(b1, b2)

    [<Properties(Arbitrary = [| typeof<OpenTKGen> |])>]
    module Size =
        [<Property>]
        let ``The size of a given box must be greater than or equal to 0`` (b : Box2) =
            Assert.True(b.Size.X * b.Size.Y >= (float32)0)

        [<Property>]
        let ``The size should follow max-min`` (b : Box2) =
           Assert.Equal(b.Size, b.Max - b.Min)

        [<Property>]
        let ``The size should be equal to the set size`` (b1 : Box2, v1 : Vector2) =
           let mutable b = b1
           let v = new Vector2(Math.Abs(v1.X), Math.Abs(v1.Y))

           b.Size <- v
           
           Assert.ApproximatelyEquivalent(v, b.Size)
        
        [<Property>]
        let ``Changing the size should not change the center`` (b1 : Box2, v1 : Vector2) =
           let mutable b = b1
           let v = b.Center
           
           b.Size <- v1
           
           Assert.ApproximatelyEquivalent(v, b.Center)
        
        [<Property>]
        let ``The halfsize should always be equal to half the size`` (b1 : Box2, v1 : Vector2) =
            let mutable b = b1
            
            b.Size <- v1
            
            Assert.Equal(b1.Size/(float32)2, b1.HalfSize)
            Assert.Equal(b.Size/(float32)2, b.HalfSize)
            
        [<Property>]
        let ``The halfsize should always be equal to half the size when using the halfSize setter`` (b1 : Box2, v1 : Vector2) =
            let mutable b = b1
            
            b.HalfSize <- v1
            
            Assert.Equal(b1.Size/(float32)2, b1.HalfSize)
            Assert.Equal(b.Size/(float32)2, b.HalfSize)

    [<Properties(Arbitrary = [| typeof<OpenTKGen> |])>]
    module Properties =
        [<Property>]
        let ``Using the properties should always result in a valid box (size >= 0)`` (v1 : Vector2, v2 : Vector2, v3 : Vector2, v4 : Vector2) =
            let mutable b = Box2(v1, v2)

            b.Min <- v3
            b.Max <- v4

            Assert.True(b.Size.X * b.Size.Y >= (float32)0)

        [<Property>]
        let ``Setting a min value higher than max moves the max`` (b1 : Box2, v1 : Vector2) =
            let mutable b = b1

            b.Min <- v1

            Assert.Equal(b.Max, Vector2.ComponentMax(v1, b1.Max))

        [<Property>]
        let ``Setting a max value lower than min moves the min`` (b1 : Box2, v1 : Vector2) =
            let mutable b = b1

            b.Max <- v1

            Assert.Equal(b.Min, Vector2.ComponentMin(v1, b1.Min))
    
    [<Properties(Arbitrary = [|typeof<OpenTKGen>|])>]
    module Scale =
        [<Property>]
        let ``When scaling the size should change accordingly`` (b1 : Box2, v1 : Vector2, v2 : Vector2) =
            let mutable b = b1
            let f = b.Size
            let v = new Vector2(Math.Abs(v1.X), Math.Abs(v1.Y))
            
            b.Scale(v, v2)
            
            Assert.ApproximatelyEqualEpsilon(v * f, b.Size, (float32)0.0001)
            
        [<Property>]
        let ``Scaling from the center of a box should have the same result as multiplying the size`` (b1 : Box2, v1 : Vector2) =
            let v2 = b1.Size * v1
            b1.Scale(v1, b1.Center)
            
            Assert.ApproximatelyEqualEpsilon(b1.Size, v2, (float32)0.0001)
