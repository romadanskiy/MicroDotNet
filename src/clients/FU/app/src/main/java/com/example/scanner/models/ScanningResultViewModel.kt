package com.example.scanner.models

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel

class ScanningResultViewModel : ViewModel() {
     var scanningResult: MutableLiveData<String> = MutableLiveData("");
}