﻿using Mono.Cecil;
using Mono.Cecil.Cil;

namespace OpenTK.Rewrite.Methods.Processors
{
    public abstract class MethodProcessorWithEpilogue<T> : IMethodProcessorWithPostProcessor
    {
        /// <inheritdoc/>
        public IMethodProcessor PostProcessor { get; }

        /// <summary>
        /// Gets the <see cref="PostProcessor"/> as an <see cref="EpilogueProcessor{T}"/>, since this class always
        /// uses this specific type for post processing.
        /// </summary>
        protected EpilogueProcessor<T> EpilogueProcessor => (EpilogueProcessor<T>)PostProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodProcessorWithEpilogue{T}"/> class with a
        /// <see cref="EpilogueProcessor{T}"/> instance that will be used for epilogue processing.
        /// </summary>
        /// <param name="epilogueProcessor">
        /// The epilogue processor instance that will be used for epilogue processing.
        /// </param>
        public MethodProcessorWithEpilogue(EpilogueProcessor<T> epilogueProcessor)
        {
            PostProcessor = epilogueProcessor;
        }

        /// <inheritdoc/>
        public abstract void Process(ILProcessor ilProcessor, MethodDefinition wrapper, MethodDefinition native);
    }
}
