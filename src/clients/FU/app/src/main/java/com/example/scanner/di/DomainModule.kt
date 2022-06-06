package com.example.scanner.di

import com.example.scanner.domain.usecase.GetGarbageInfoUseCase
import org.koin.dsl.module

val domainModule = module {

    factory {
        GetGarbageInfoUseCase(garbageRepository = get())
    }
}