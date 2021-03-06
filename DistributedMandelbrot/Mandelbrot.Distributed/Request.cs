﻿using System;

namespace Mandelbrot.Distributed.Server
{
    public class Request
    {
        #region Constructors

        public Request(
            int id,
            double realCenter,
            double imaginaryCenter,
            double width,
            double height,
            double resolution,
            int maxIterations,
            int maxMagnitude)
        {
            Id = id;
            RealCenter = realCenter;
            ImaginaryCenter = imaginaryCenter;
            Width = width;
            Height = height;
            Resolution = resolution;
            MaxIterations = maxIterations;
            MaxMagnitude = maxMagnitude;
        }

        #endregion

        #region Properties
        
        public int Id { get; }
        public double RealCenter { get; }
        public double ImaginaryCenter { get; }
        public double Width { get; }
        public double Height { get; }
        public double Resolution { get; }
        public int MaxIterations { get; }
        public int MaxMagnitude { get; }

        public double RealLowerBound => RealCenter - Width / 2;
        public double ImaginaryLowerBound => ImaginaryCenter - Height / 2;
        
        public int WidthPixels => (int)Math.Ceiling(Width * (1.0/ Resolution));
        public int HeightPixels => (int)Math.Ceiling(Height * (1.0 / Resolution));

        public int ResultPixelCount => WidthPixels * HeightPixels;

        #endregion


    }
}