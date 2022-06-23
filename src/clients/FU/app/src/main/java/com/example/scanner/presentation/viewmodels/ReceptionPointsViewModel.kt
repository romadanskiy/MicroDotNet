package com.example.scanner.presentation.viewmodels

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.scanner.domain.models.GarbageCategory
import com.example.scanner.domain.models.GetReceptionPoint
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.usecase.GetReceptionPointsUseCase
import com.example.scanner.presentation.models.GarbageCategoryModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class ReceptionPointsViewModel(val getReceptionPointsUseCase: GetReceptionPointsUseCase) : LoadedViewModel() {
    private val categoryIdsMutable: MutableLiveData<List<Long>> = MutableLiveData()
    val categoryIds: LiveData<List<Long>> = categoryIdsMutable
    private val pointsResultMutable: MutableLiveData<RequestResult<GetReceptionPoint>> =
        MutableLiveData()
    val pointsResult: LiveData<RequestResult<GetReceptionPoint>> = pointsResultMutable

    fun getPoints() {
        viewModelScope.launch(Dispatchers.IO) {
            var result = getReceptionPointsUseCase.execute(categoryIds.value)
            pointsResultMutable.postValue(result)
            loadingMutable.postValue(false)
        }
    }

    fun setIds(ids: List<Long>){
        categoryIdsMutable.value = ids
    }


    override fun clear() {
        super.clear()
        categoryIdsMutable.postValue(mutableListOf())
        pointsResultMutable.postValue(null)
        setLoading(false)
    }
}