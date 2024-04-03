using System;

namespace ConsoleAppZipper.Output
{
    public class OutputFactory
    {
        private static OutputFactory instance;
        private static readonly object lockObject = new object();

        private OutputFactory() { }

        public static OutputFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        instance ??= new OutputFactory();
                    }
                }

                return instance;
            }
        }

        public IOutput CreateOutput(OutputType outputType, string additionalParameter)
        {
            return outputType switch
            {
                OutputType.LocalFile => new LocalFileOutput(additionalParameter),
                OutputType.FileShare => new FileShareOutput(additionalParameter),
                OutputType.SMTP => CreateSmtpOutput(additionalParameter),
                _ => throw new ArgumentException("Invalid output type"),
            };
        }

        #region Private Methods
        private IOutput CreateSmtpOutput(string additionalParameter)
        {
            if (string.IsNullOrEmpty(additionalParameter) ||
                (additionalParameter.Split(',').Length != 4))
            {
                throw new ArgumentException("SMTP parameters are missing or invalid.");
            }

            string[] additionalParameters = additionalParameter.Split(',');

            return new SmtpOutput(
                additionalParameters[0],
                additionalParameters[1],
                additionalParameters[2],
                additionalParameters[3]);
        }
        #endregion
    }
}