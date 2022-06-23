package com.example.scanner.presentation.viewmodels

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.scanner.ApiRequestResult
import com.example.scanner.domain.models.GarbageCategory
import com.example.scanner.domain.models.RequestResult
import com.example.scanner.domain.usecase.GetGarbageCategoriesUseCase
import com.example.scanner.presentation.models.GarbageCategoryModel
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class GarbageCategoriesViewModel(val getGarbageCategoriesUseCase: GetGarbageCategoriesUseCase) :
    LoadedViewModel() {
    private val categoriesUIMutable: MutableLiveData<List<GarbageCategoryModel>> = MutableLiveData()
    val categoriesUI: LiveData<List<GarbageCategoryModel>> = categoriesUIMutable
    private val categoryIdsMutable: MutableLiveData<MutableList<Long>> = MutableLiveData()
    val categoryIds: LiveData<MutableList<Long>> = categoryIdsMutable
    private val categoriesResultMutable: MutableLiveData<RequestResult<GarbageCategory>> =
        MutableLiveData()
    val categoriesResult: LiveData<RequestResult<GarbageCategory>> = categoriesResultMutable

    fun getCategories() {
        viewModelScope.launch(Dispatchers.IO) {
            var result = getGarbageCategoriesUseCase.execute()
            categoriesResultMutable.postValue(result)
        }
        loadingMutable.postValue(false)
    }

    fun setCategories(categories: List<GarbageCategory>) {
        categoriesUIMutable.postValue(List(categories.size) {
            GarbageCategoryModel(categories[it].name, categories[it].id, false)
        })
        if(categoryIdsMutable.value == null){
            categoryIdsMutable.postValue(mutableListOf())
        }
    }

    fun setCategoryIds() {
        if(categoryIdsMutable.value == null){
            categoryIdsMutable.value  = mutableListOf()
        }
    }

    fun removeSelectedCategory(categoryId: Long){
        categoryIdsMutable.value?.remove(categoryId)
    }

    fun addSelectedCategory(categoryId: Long){
        categoryIdsMutable.value?.add(categoryId)
    }

    override fun clear() {
        categoriesUIMutable.postValue(emptyList())
        categoryIdsMutable.postValue(mutableListOf())
        categoriesResultMutable.postValue(null)
        super.clear()
    }
}