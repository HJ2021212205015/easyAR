# ezAR AR教育应用实现计划

## Context

这是一个基于EasyAR的Unity AR教育应用，**目标平台为PC端**，帮助学生进行虚拟模型的构建、测量和分析。当前项目已有基础框架（图像追踪、模型加载、测量系统），但缺少完整的UI交互和部分核心功能。本计划旨在完善所有需求功能，打造一个完整可用的AR教学工具。

**关键变更**: 由于是PC端应用，交互方式从触摸手势改为鼠标+键盘操作。

---

## 现状分析

### 已实现功能

| 功能 | 文件 | 状态 |
|------|------|------|
| 图像追踪锚点 | `ImageTargetContentAnchor.cs` | ✅ 基础完成 |
| STL模型加载 | `ModelLoader.cs` + `StlImporter.cs` | ✅ 完成 |
| 模型变换API | `ModelTransformController.cs` | ✅ API完成，无交互 |
| 透明度控制 | `ModelLoader.SetModelOpacity()` | ✅ 完成 |
| 测量系统 | `MeasurementSystem.cs` | ✅ 基础完成，无UI交互 |

### 需要新增的功能

1. **② 模型操控** - 需集成鼠标交互
2. **③ 基础几何体搭建** - 全新功能
3. **④ 距离测量** - 需添加UI控制和鼠标选点
4. **⑤ 画面冻结模式** - 需新增实现
5. **⑥ 透明度调节** - 需添加UI控制
6. **⑦ 2D涂鸦标注** - 全新功能（PC端鼠标绘制）

---

## 实现方案

### 新增文件结构

```
Assets/Scripts/
├── Core/
│   ├── ARManager.cs              # AR会话管理（单例）
│   ├── AppModeManager.cs         # 模式管理（AR/冻结/涂鸦）
│   └── InputManager.cs           # 统一输入分发
├── Model/
│   └── ModelMouseHandler.cs      # 模型鼠标交互
├── Measurement/
│   ├── MeasurementUIController.cs # 测量UI控制
│   └── MeasurementMouseHandler.cs # 测量鼠标交互
├── Geometry/
│   ├── GeometryBuilder.cs        # 几何体构建系统
│   └── BoundingBoxCalculator.cs  # 包围盒计算
├── Rendering/
│   └── FrameFreezer.cs           # 画面冻结控制
└── Drawing/
    ├── DrawingCanvas.cs          # 2D涂鸦画布
    ├── BrushController.cs        # 画笔控制
    └── ScreenshotSaver.cs        # 截图保存
```

---

### 功能① 虚实精准对齐

**现状**: `ImageTargetContentAnchor.cs` 已实现TargetFound/Lost处理和localOffset设置。

**改进点**:
- 添加手动微调接口（通过UI滑块调整偏移）
- 支持校准参数保存

**修改文件**: `ImageTargetContentAnchor.cs`

---

### 功能② 模型操控（PC端鼠标交互）

**实现方式**: 鼠标拖拽 + 键盘快捷键

**交互逻辑**:
- **鼠标左键拖拽** → 模型平面移动（XZ平面）
- **鼠标左键 + Shift** → 模型上下移动（Y轴）
- **鼠标右键拖拽** → 模型旋转
- **鼠标滚轮** → 模型缩放
- **R键** → 重置变换

**新增文件**: `ModelMouseHandler.cs`

**核心代码**:
```csharp
void Update()
{
    // 鼠标左键拖拽
    if (Input.GetMouseButton(0))
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Y轴移动
            float deltaY = Input.GetAxis("Mouse Y") * moveSpeed;
            transformController.MoveModelY(deltaY);
        }
        else
        {
            // XZ平面移动
            float deltaX = Input.GetAxis("Mouse X") * moveSpeed;
            float deltaZ = Input.GetAxis("Mouse Y") * moveSpeed;
            transformController.MoveModel(new Vector3(deltaX, 0, deltaZ));
        }
    }

    // 鼠标右键旋转
    if (Input.GetMouseButton(1))
    {
        float rotX = Input.GetAxis("Mouse Y") * rotateSpeed;
        float rotY = Input.GetAxis("Mouse X") * rotateSpeed;
        transformController.RotateModel(new Vector3(-rotX, rotY, 0));
    }

    // 滚轮缩放
    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if (scroll != 0)
    {
        transformController.ScaleModel(scroll > 0 ? 1 : -1);
    }
}
```

---

### 功能③ 基础几何体搭建

**新增文件**: `GeometryBuilder.cs`

**核心功能**:
- 创建参数化几何体（圆柱、长方体、球体）
- 几何体变换控制（位置、旋转、缩放）
- 包围盒计算与显示
- 导出设计数据（JSON格式）

**API设计**:
```csharp
public GameObject CreatePrimitive(PrimitiveType type, GeometryParams parameters);
public BoundingBoxData CalculateBoundingBox();
public DesignData ExportDesignData();
```

---

### 功能④ 距离测量（PC端鼠标交互）

**增强现有 `MeasurementSystem.cs`**:

**新增方法**:
```csharp
public void SetPointAPosition(Vector3 worldPosition);
public void SetPointBPosition(Vector3 worldPosition);
public void SetGridVisible(bool visible);
```

**新增文件**: `MeasurementUIController.cs`

**UI组件**:
- 网格高度滑块 (0-50cm)
- 记录点A/B按钮（或鼠标点击选点）
- 测量结果显示
- 清除测量按钮

**鼠标选点交互**:
- 左键点击 → 记录测量点
- 先点击位置记录点A，再次点击记录点B
- 网格高度通过UI滑块调整

**冻结模式下的测量**:
- 冻结画面后，通过射线检测确定鼠标点击位置
- 在冻结平面上记录测量点

---

### 功能⑤ 画面冻结模式

**新增文件**: `FrameFreezer.cs`

**实现原理** (参考 `Coloring3D.cs`):
```csharp
// 获取相机渲染纹理
Session.Assembly.CameraImageRenderer.Value.RequestTargetTexture((_, texture) =>
    renderTexture = texture);

// 冻结：复制当前帧到新纹理
freezedTexture = new RenderTexture(renderTexture.width, renderTexture.height, 0);
Graphics.Blit(renderTexture, freezedTexture);

// 解冻：销毁冻结纹理，恢复AR追踪
Destroy(freezedTexture);
```

**状态管理**:
- `AppModeManager` 管理 AR/冻结/涂鸦 三种模式
- 冻结模式下禁用AR追踪，启用测量/涂鸦功能

---

### 功能⑥ 透明度调节

**现状**: `ModelLoader.SetModelOpacity(float opacity)` 已实现。

**新增文件**: `OpacityController.cs` (UI控制器)

**UI组件**:
- 透明度滑块 (0-100%)
- X光模式快捷按钮 (一键设为30%透明)
- 重置按钮

---

### 功能⑦ 冻结画面上的2D涂鸦标注（PC端鼠标绘制）

**新增文件**: `DrawingCanvas.cs`, `BrushController.cs`, `ScreenshotSaver.cs`

**实现方案**:
1. 创建全屏 RawImage 作为绘图画布
2. 使用 Texture2D 存储绘制内容
3. 监听鼠标事件，在纹理上绘制
4. 保存时合并冻结画面 + 涂鸦层

**鼠标绘制交互**:
- 鼠标左键拖拽 → 绘制线条
- 鼠标右键 → 橡皮擦
- 滚轮 → 调整画笔大小

**保存到文件** (PC端):
```csharp
// 保存到用户指定路径或默认路径
string defaultPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
    $"ezAR_Annotation_{DateTime.Now:yyyyMMdd_HHmmss}.png"
);
File.WriteAllBytes(defaultPath, drawingTexture.EncodeToPNG());
```

---

## 关键文件修改清单

| 文件 | 操作 | 说明 |
|------|------|------|
| `ImageTargetContentAnchor.cs` | 修改 | 添加手动微调接口 |
| `MeasurementSystem.cs` | 修改 | 添加鼠标选点接口 |
| `ModelMouseHandler.cs` | 新增 | 模型鼠标交互 |
| `MeasurementUIController.cs` | 新增 | 测量UI控制 |
| `FrameFreezer.cs` | 新增 | 画面冻结 |
| `GeometryBuilder.cs` | 新增 | 几何体构建 |
| `OpacityController.cs` | 新增 | 透明度UI控制 |
| `DrawingCanvas.cs` | 新增 | 涂鸦画布（鼠标绘制） |
| `AppModeManager.cs` | 新增 | 应用模式管理 |

---

## 实现优先级

| 优先级 | 功能 | 预估工时 |
|--------|------|----------|
| P0 | ② 模型操控（鼠标交互） | 2天 |
| P0 | ⑤ 画面冻结模式 | 1天 |
| P1 | ⑥ 透明度调节 | 0.5天 |
| P1 | ④ 距离测量（UI+鼠标） | 2天 |
| P2 | ③ 基础几何体搭建 | 3天 |
| P3 | ⑦ 2D涂鸦标注 | 3天 |

---

## 验证方法

### 单元测试
- 每个新增组件独立测试功能

### 集成测试（PC端）
1. **图像追踪测试**: 使用摄像头扫描识别图，确认虚拟模型正确锚定
2. **模型操控测试**: 鼠标左键移动、右键旋转、滚轮缩放
3. **测量测试**: 调整网格高度，鼠标点击选点，验证距离计算
4. **冻结测试**: 冻结画面后进行测量和涂鸦
5. **透明度测试**: 调节滑块验证模型透明度变化
6. **涂鸦测试**: 冻结后鼠标绘制标注，保存到图片文件夹

### 测试环境
- Windows PC + 外接摄像头（或笔记本内置摄像头）

---

## 复用资源

- **Coloring3D.cs** (`Assets/Samples/.../Coloring3D.cs`) - 画面冻结参考实现
- **ModelLoader.SetModelOpacity()** - 已有的透明度控制
- **MeasurementSystem.cs** - 已有的测量网格和计算逻辑

---

## 实现进度追踪

> 此部分将在开发过程中实时更新

### 功能① 虚实精准对齐
- [ ] 添加手动微调接口
- [ ] 校准参数保存

### 功能② 模型操控
- [ ] ModelMouseHandler.cs 创建
- [ ] 鼠标左键移动
- [ ] 鼠标右键旋转
- [ ] 滚轮缩放
- [ ] 键盘快捷键

### 功能③ 基础几何体搭建
- [ ] GeometryBuilder.cs 创建
- [ ] 参数化几何体创建
- [ ] 包围盒计算
- [ ] 设计数据导出

### 功能④ 距离测量
- [ ] MeasurementSystem增强
- [ ] MeasurementUIController创建
- [ ] 鼠标选点交互
- [ ] 冻结模式测量

### 功能⑤ 画面冻结模式
- [ ] FrameFreezer.cs 创建
- [ ] 冻结/解冻功能
- [ ] AppModeManager模式管理

### 功能⑥ 透明度调节
- [ ] OpacityController.cs 创建
- [ ] UI滑块集成

### 功能⑦ 2D涂鸦标注
- [ ] DrawingCanvas.cs 创建
- [ ] 鼠标绘制功能
- [ ] 截图保存功能
