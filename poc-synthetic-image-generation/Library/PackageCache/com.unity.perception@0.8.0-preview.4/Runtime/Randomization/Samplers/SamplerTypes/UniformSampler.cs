﻿using System;

namespace UnityEngine.Perception.Randomization.Samplers
{
    /// <summary>
    /// Returns uniformly distributed random values within a designated range.
    /// </summary>
    [Serializable]
    public class UniformSampler : ISampler
    {
        /// <summary>
        /// A range bounding the values generated by this sampler
        /// </summary>
        public FloatRange range;

        /// <summary>
        /// Constructs a UniformSampler
        /// </summary>
        public UniformSampler()
        {
            range = new FloatRange(0f, 1f);
        }

        /// <summary>
        /// Constructs a new uniform distribution sampler
        /// </summary>
        /// <param name="min">The smallest value contained within the range</param>
        /// <param name="max">The largest value contained within the range</param>
        public UniformSampler(float min, float max)
        {
            range = new FloatRange(min, max);
        }

        /// <summary>
        /// Generates one sample
        /// </summary>
        /// <returns>The generated sample</returns>
        public float Sample()
        {
            var rng = SamplerState.CreateGenerator();
            return rng.NextFloat(range.minimum, range.maximum);
        }

        /// <summary>
        /// Validates that the sampler is configured properly
        /// </summary>
        public void Validate()
        {
            range.Validate();
        }
    }
}
