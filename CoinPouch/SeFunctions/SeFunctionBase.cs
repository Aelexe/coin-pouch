﻿using System;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Hooking;

namespace CoinPouch.SeFunctions {
    public class SeFunctionBase<T> where T : Delegate {
        public IntPtr Address;
        protected T? FuncDelegate;

        public SeFunctionBase(SigScanner sigScanner, int offset) {
            Address = sigScanner.Module.BaseAddress + offset;
        }

        public SeFunctionBase(SigScanner sigScanner, string signature, int offset = 0) {
            Address = sigScanner.ScanText(signature);
            if(Address != IntPtr.Zero) {
                Address += offset;
            }
        }

        public T? Delegate() {
            if(FuncDelegate != null) {
                return FuncDelegate;
            }

            if(Address != IntPtr.Zero) {
                FuncDelegate = Marshal.GetDelegateForFunctionPointer<T>(Address);
                return FuncDelegate;
            }

            return null;
        }

        public dynamic? Invoke(params dynamic[] parameters) {
            if(FuncDelegate != null) {
                return FuncDelegate.DynamicInvoke(parameters);
            }

            if(Address != IntPtr.Zero) {
                FuncDelegate = Marshal.GetDelegateForFunctionPointer<T>(Address);
                return FuncDelegate!.DynamicInvoke(parameters);
            } else {
                return null;
            }
        }

        public Hook<T>? CreateHook(T detour) {
            if(Address != IntPtr.Zero) {
                var hook = new Hook<T>(Address, detour);
                hook.Enable();
                return hook;
            }

            return null;
        }
    }
}
