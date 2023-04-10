using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

public class ExceptionHandler
{
    public event EventHandler<ExceptionEventArgs> OnExceptionСatch;

    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception;
        public string ExceptionDescription;
    }

    private readonly Dictionary<ExceptionType, Action<Exception>> _exceptionHandlers;

    public ExceptionHandler()
    {
        _exceptionHandlers = new()
        {
            { ExceptionType.WebException, ex => DescribeException("WebException occurred ", ex) },
            { ExceptionType.HttpRequestException, ex => DescribeException("HttpRequestException occurred ", ex) },
            { ExceptionType.JsonReaderException, ex => DescribeException("JsonReaderException occurred ", ex) },
            { ExceptionType.JsonSerializationException, ex => DescribeException("JsonSerializationException occurred ", ex) },
            { ExceptionType.DecoderFallbackException, ex => DescribeException("DecoderFallbackException occurred ", ex) },
            { ExceptionType.EncoderFallbackException, ex => DescribeException("EncoderFallbackException occurred ", ex) },
            { ExceptionType.AuthenticationException, ex => DescribeException("AuthenticationException occurred ", ex) },
            { ExceptionType.CryptographicException, ex => DescribeException("CryptographicException occurred ", ex) },
            { ExceptionType.ArgumentNullException, ex => DescribeException("ArgumentNullException exception occurred ", ex) },
            { ExceptionType.Unknown, ex => DescribeException("Unknow exception ", ex) }
        };
    }

    public void HandleException(Exception ex)
    {
        if (_exceptionHandlers.TryGetValue(GetExceptionType(ex), out Action<Exception> handler))
            handler(ex);
    }

    private ExceptionType GetExceptionType(Exception ex)
    {
        return ex switch
        {
            WebException _ => ExceptionType.WebException,
            HttpRequestException _ => ExceptionType.HttpRequestException,
            JsonReaderException _ => ExceptionType.JsonReaderException,
            JsonSerializationException _ => ExceptionType.JsonSerializationException,
            DecoderFallbackException _ => ExceptionType.DecoderFallbackException,
            EncoderFallbackException _ => ExceptionType.EncoderFallbackException,
            AuthenticationException _ => ExceptionType.AuthenticationException,
            CryptographicException _ => ExceptionType.CryptographicException,
            ArgumentNullException _ => ExceptionType.ArgumentNullException,
            _ => ExceptionType.Unknown,
        };
    }

    private void DescribeException(string descritpionException, Exception exception)
    {
        OnExceptionСatch?.Invoke(this, new ExceptionEventArgs
        {
            Exception = exception,
            ExceptionDescription = descritpionException,
        });
    }
}
