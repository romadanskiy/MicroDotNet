package com.example.scanner.di

import com.example.scanner.presentation.viewmodels.ScannerViewModel
import org.koin.androidx.viewmodel.dsl.viewModel
import org.koin.dsl.module

val appModule = module{
    viewModel<ScannerViewModel>()
}