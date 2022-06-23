package com.example.scanner.di

import com.example.scanner.domain.usecase.*
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

    factory {
        GetReceptionPointsUseCase(receptionPointRepository = get())
    }

    factory {
        AddReceptionPointUseCase(receptionPointRepository = get())
    }
}