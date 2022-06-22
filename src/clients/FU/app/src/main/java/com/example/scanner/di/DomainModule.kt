package com.example.scanner.di

import com.example.scanner.domain.usecase.AddGarbageInfoUseCase
import com.example.scanner.domain.usecase.GetGarbageCategoriesUseCase
import com.example.scanner.domain.usecase.GetGarbageInfoUseCase
import com.example.scanner.domain.usecase.LoadImageUseCase
import org.koin.dsl.module

val domainModule = module {

    factory<GetGarbageInfoUseCase> {
        GetGarbageInfoUseCase(garbageRepository = get())
    }

    factory<AddGarbageInfoUseCase> {
        AddGarbageInfoUseCase(garbageRepository = get())
    }

    factory {
        LoadImageUseCase(imageRepository = get())
    }

    factory {
        GetGarbageCategoriesUseCase(garbageRepository = get())
    }
}