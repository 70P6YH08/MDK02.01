using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PractWorkLibrary.DTOs;
using PractWorkLibrary.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PractLibrary.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private UserService _userService;

        [ObservableProperty]
        private ObservableCollection<UserDto> _userDtos = new();

        public MainViewModel(UserService userService)
        {
            _userService = userService;
            _userDtos = _userService.GetUserDtosAsync();
        }

        [RelayCommand]

    }
}
