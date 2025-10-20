#!/bin/bash

# Hashtag Generator API - cURL Examples
# Make sure the API is running on http://localhost:5066

echo "=== Test 1: Generate 8 hashtags with specific model ==="
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"text":"Exploring the beautiful beaches of Rio de Janeiro with amazing sunset views and delicious Brazilian food","count":8,"model":"llama3.2:3b"}' \
  | json_pp
echo -e "\n"

echo "=== Test 2: Generate hashtags with default count (10) ==="
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"text":"Learning .NET 8 Minimal APIs and integrating with AI models for innovative solutions","model":"llama3.2:3b"}' \
  | json_pp
echo -e "\n"

echo "=== Test 3: Generate 5 hashtags ==="
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"text":"Technology innovation, artificial intelligence, machine learning, and the future of software development","count":5,"model":"llama3.2:3b"}' \
  | json_pp
echo -e "\n"

echo "=== Test 4: Test with maximum count (30) ==="
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"text":"Climate change awareness, sustainability, renewable energy, environmental protection, green technology","count":30,"model":"llama3.2:3b"}' \
  | json_pp
echo -e "\n"

echo "=== Test 5: Test with count > 30 (should cap at 30) ==="
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"text":"Digital marketing strategies for social media growth and engagement","count":50,"model":"llama3.2:3b"}' \
  | json_pp
echo -e "\n"

echo "=== Test 6: Missing count (should default to 10) ==="
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"text":"Travel adventures around the world, exploring new cultures and cuisines"}' \
  | json_pp
echo -e "\n"

echo "=== Test 7: Error - Empty text ==="
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"text":"","count":5,"model":"llama3.2:3b"}' \
  | json_pp
echo -e "\n"

echo "=== Test 8: Error - Missing text ==="
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"count":5,"model":"llama3.2:3b"}' \
  | json_pp
echo -e "\n"

echo "=== Test 9: Portuguese text ==="
curl -X POST http://localhost:5066/hashtags \
  -H "Content-Type: application/json" \
  -d '{"text":"Desenvolvimento de software com inteligência artificial e aprendizado de máquina usando .NET","count":7,"model":"llama3.2:3b"}' \
  | json_pp
echo -e "\n"
