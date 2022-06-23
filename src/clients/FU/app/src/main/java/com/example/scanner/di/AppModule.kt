package com.example.scanner.di

import com.example.scanner.domain.usecase.GetGarbageInfoUseCase
import com.example.scanner.presentation.viewmodels.*
import org.koin.androidx.viewmodel.dsl.viewModel
import org.koin.dsl.module

val appModule = module{
    viewModel<ScannerViewModel>()
    viewModel<ErrorViewModel>()

    viewModel<ScanningResultViewModel> {
        ScanningResultViewModel(getGarbageInfoUseCase = get(), loadImageUseCase = get())
    }

    viewModel<AddGarbageViewModel>(){
        AddGarbageViewModel(addGarbageInfoUseCase = get(), loadImageUseCase = get())
    }

    viewModel(){
        GarbageCategoriesViewModel(getGarbageCategoriesUseCase = get())
    }

    viewModel(){
        ReceptionPointsViewModel(getReceptionPointsUseCase = get())
    }

    viewModel(){
        AddReceptionPointViewModel(addReceptionPointUseCase = get())
    }
}