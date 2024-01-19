namespace MiddlewareLoginAssignment.Exceptions
{
    public class IncorrectUserNameException : Exception
    {
        public IncorrectUserNameException()
        {

        }

        public IncorrectUserNameException(string message) : base(message)
        {
            
        }

        public IncorrectUserNameException(string message, Exception inner) : base(message, inner)
        {



        }
    }

    public class NullUserNameException : Exception
    {
        public NullUserNameException()
        {

        }

        public NullUserNameException(string message) : base(message)
        {

        }

        public NullUserNameException(string message, Exception inner) : base(message, inner)
        {



        }
    }
}
