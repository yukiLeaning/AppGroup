using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloom.DataObjects.Enums
{
    public enum ApplicationErrorCode_Enum
    {
        Invalid = 0x0000,   //未定義
        Success = 0x0001,   //成功

        FatalError          = 0xFFFF,   //致命的エラー
        UnknownError        = 0xFFFE,   //不明なエラー
        ParameterError      = 0xFFFD,   //パラメータエラー
        LogicError          = 0xFFFC,   //ロジックエラー
        OutOfLangeError     = 0xFFFB,   //OutOfLangeエラー
    }
}
