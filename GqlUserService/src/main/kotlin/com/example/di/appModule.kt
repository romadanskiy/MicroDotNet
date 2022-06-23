package com.example.di

import org.koin.dsl.module

val appModule = module(createdAtStart = true) {
//    single<RatingGRPCService> { RatingGRPCService() } DbService?
}