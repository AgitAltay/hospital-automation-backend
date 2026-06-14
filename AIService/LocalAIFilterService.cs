using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using AIService.Interface;

namespace AIService;

public class TextData
{
    [LoadColumn(0)]
    public string Text { get; set; }

    [LoadColumn(1)]
    public bool IsMeaningful { get; set; }
}

public class TextPrediction
{
    [ColumnName("PredictedLabel")]
    public bool IsMeaningful { get; set; }
}

public class LocalAIFilterService : ILocalAIFilterService
{
    private readonly MLContext _mlContext;
    private ITransformer _model;
    private PredictionEngine<TextData, TextPrediction> _predictionEngine;

    public LocalAIFilterService()
    {
        _mlContext = new MLContext();
        TrainModel();
    }

    private void TrainModel()
    {
        // Dosya yolunu belirle (Development ortamında çalışırken kök dizinde veya bin içerisinde olabilir)
        var projectDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "training_data.csv");
        var binDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "training_data.csv");
        
        string dataPath = null;
        if (File.Exists(projectDataPath))
            dataPath = projectDataPath;
        else if (File.Exists(binDataPath))
            dataPath = binDataPath;

        if (dataPath == null)
        {
            // Veri bulunamazsa konsola log yazıp modeli eğitmeden çık (her şeye true der)
            Console.WriteLine("[LocalAIFilterService] UYARI: training_data.csv bulunamadı. Filtreleme devre dışı.");
            return;
        }

        try
        {
            IDataView dataView = _mlContext.Data.LoadFromTextFile<TextData>(
                path: dataPath, 
                hasHeader: true, 
                separatorChar: ',',
                allowQuoting: true); // CSV içindeki tırnak işaretlerini (",") doğru okuması için

            var pipeline = _mlContext.Transforms.Text.FeaturizeText(
                    outputColumnName: "Features", 
                    inputColumnName: nameof(TextData.Text))
                .Append(_mlContext.BinaryClassification.Trainers.FastTree(
                    labelColumnName: nameof(TextData.IsMeaningful), 
                    featureColumnName: "Features"));

            // Modeli eğit (Çok hızlıdır, 1 saniyeden az sürer)
            _model = pipeline.Fit(dataView);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<TextData, TextPrediction>(_model);
            Console.WriteLine("[LocalAIFilterService] Model başarıyla eğitildi ve yüklendi.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LocalAIFilterService] Model eğitimi sırasında hata: {ex.Message}");
        }
    }

    public bool IsMeaningful(string text)
    {
        // Eğer model yüklenemediyse veya metin boşsa geçişine izin ver
        if (_predictionEngine == null || string.IsNullOrWhiteSpace(text))
        {
            return true; 
        }

        var input = new TextData { Text = text };
        var prediction = _predictionEngine.Predict(input);
        
        return prediction.IsMeaningful;
    }
}
