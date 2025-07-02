from transformers import AutoModelForCausalLM, AutoTokenizer, pipeline

# Загружаем обученную модель
model_path = "C:/Users/cody/UniTrip/UniTrip/Model/innopolis_bot_model"

tokenizer = AutoTokenizer.from_pretrained(model_path)
model = AutoModelForCausalLM.from_pretrained(model_path)

# Создаем пайплайн генерации
chatbot = pipeline("text-generation", model=model, tokenizer=tokenizer)

# Пример запроса
prompt = "Когда был основан Университет Иннополис?"
output = chatbot(prompt, max_new_tokens=200, do_sample=False)
print(output[0]['generated_text'])
