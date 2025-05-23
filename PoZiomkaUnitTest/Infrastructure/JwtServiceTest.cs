﻿using PoZiomkaDomain.Common.Interface;
using PoZiomkaInfrastructure.Services;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Infrastructure;

public class JwtServiceTest
{
    [Fact]
    public async Task ClaimDecodesCorrectly()
    {
        JwtService jwtService = new("pwWwfuP1E0Y0fXfR1Cay7EgWibc8jSrS", "issuer", "audience");

        var token = await jwtService.GenerateToken(new ClaimsIdentity([new Claim("key", "value")]), TimeSpan.FromMinutes(20));

        var identity = await jwtService.ReadToken(token);

        Assert.True(identity.HasClaim("key", "value"));
    }

    [Fact]
    public async Task DifferentIssuersThrowsError()
    {
        JwtService jwtService1 = new("pwWwfuP1E0Y0fXfR1Cay7EgWibc8jSrS", "issuer1", "audience");
        JwtService jwtService2 = new("pwWwfuP1E0Y0fXfR1Cay7EgWibc8jSrS", "issuer2", "audience");

        var token = await jwtService1.GenerateToken(new ClaimsIdentity([new Claim("key", "value")]), TimeSpan.FromMinutes(20));

        await Assert.ThrowsAsync<TokenValidationException>(() => jwtService2.ReadToken(token));
    }

    [Fact]
    public async Task DifferentAudiencesThrowsError()
    {
        JwtService jwtService1 = new("pwWwfuP1E0Y0fXfR1Cay7EgWibc8jSrS", "issuer", "audience1");
        JwtService jwtService2 = new("pwWwfuP1E0Y0fXfR1Cay7EgWibc8jSrS", "issuer", "audience2");

        var token = await jwtService1.GenerateToken(new ClaimsIdentity([new Claim("key", "value")]), TimeSpan.FromMinutes(20));

        await Assert.ThrowsAsync<TokenValidationException>(() => jwtService2.ReadToken(token));
    }

    [Fact]
    public async Task DifferentKeyThrowsError()
    {
        JwtService jwtService1 = new("pwWwfuP1E0Y0fXfR1Cay7EgWibc8jSrS", "issuer", "audience");
        JwtService jwtService2 = new("wwwwwwP1E0Y0fXfR1Cay7EgWibc8jSrS", "issuer", "audience");

        var token = await jwtService1.GenerateToken(new ClaimsIdentity([new Claim("key", "value")]), TimeSpan.FromMinutes(20));

        await Assert.ThrowsAsync<TokenValidationException>(() => jwtService2.ReadToken(token));
    }

    [Fact]
    public async Task ExpiredThrowsError()
    {
        JwtService jwtService = new("pwWwfuP1E0Y0fXfR1Cay7EgWibc8jSrS", "issuer", "audience");

        var token = await jwtService.GenerateToken(new ClaimsIdentity([new Claim("key", "value")]), TimeSpan.Zero);

        await Assert.ThrowsAsync<TokenExpiredException>(() => jwtService.ReadToken(token));
    }
}
