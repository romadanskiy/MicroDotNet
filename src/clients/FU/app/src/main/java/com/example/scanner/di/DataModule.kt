package com.example.scanner.di

import com.example.scanner.data.repository.GarbageRepositoryImpl
import com.example.scanner.data.repository.ImageRepositoryImpl
import com.example.scanner.data.repository.ReceptionPointRepositoryImpl
import com.example.scanner.data.storage.GarbageStorage
import com.example.scanner.data.storage.ReceptionPointStorage
import com.example.scanner.data.storage.retrofit2.GarbageStorageImpl
import com.example.scanner.data.storage.retrofit2.ReceptionPointStorageImpl
import com.example.scanner.domain.repository.GarbageRepository
import com.example.scanner.domain.repository.ImageRepository
import com.example.scanner.domain.repository.ReceptionPointRepository
import org.koin.dsl.module
import kotlin.math.sin

val dataModule = module {

    single<GarbageRepository> {
        GarbageRepositoryImpl(garbageStorage = get())
    }

    single<ImageRepository> {
        ImageRepositoryImpl()
    }

    single<ReceptionPointRepository> {
        ReceptionPointRepositoryImpl(receptionPointStorage = get())
    }

    single<GarbageStorage> {
        GarbageStorageImpl()
    }

    single<ReceptionPointStorage>{
        ReceptionPointStorageImpl()
    }
}