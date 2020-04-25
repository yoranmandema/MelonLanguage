using System;
using System.Collections.Generic;
using System.Text;

namespace MelonLanguage.Runtime {
    public class MelonException : Exception {
        public MelonException() : base() {
        }

        public MelonException(string message) : base(message) {
        }

        public MelonException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
